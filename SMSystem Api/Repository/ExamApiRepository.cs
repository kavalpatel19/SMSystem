using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Migrations;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Model.Fees;
using SMSystem_Api.Repository.Interfaces;
using System.Data;

namespace SMSystem_Api.Repository
{
    public class ExamApiRepository : IExamApiRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;

        public ExamApiRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.Configuration = configuration;
        }

        public List<ExamModel> GetAllExams()
        {
            var Data = context.Exams.Where(x => x.IsActive).ToList();
            return Data;
        }

        public async Task<PaggedExamModel> GetAll(SearchingPara para)
        {
            string connaction = Configuration.GetConnectionString("connaction");

            List<ExamModel> data = new List<ExamModel>();

            const int pagesize = 5;

            int totalPage = 0;

            using (SqlConnection con = new SqlConnection(connaction))
            {
                SqlCommand cmd = new SqlCommand("ExamPaging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@PageIndex", SqlDbType.VarChar).Value = para.PageIndex;
                cmd.Parameters.Add("@PageSize", SqlDbType.VarChar).Value = pagesize;
                cmd.Parameters.Add("@TotalPages", SqlDbType.Int, 4);
                cmd.Parameters["@TotalPages"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                con.Close();

                totalPage = Convert.ToInt32(cmd.Parameters["@TotalPages"].Value);

                using (SqlConnection conn = new SqlConnection(connaction))
                {
                    DataSet ds = new DataSet();

                    conn.Open();
                    SqlDataAdapter rdr = new SqlDataAdapter();
                    rdr.SelectCommand = cmd;
                    rdr.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ExamModel exam = new ExamModel();

                        exam.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        exam.ExamName = ds.Tables[0].Rows[i]["ExamName"].ToString();
                        exam.Class = ds.Tables[0].Rows[i]["Class"].ToString();
                        exam.Subject = ds.Tables[0].Rows[i]["Subject"].ToString();
                        exam.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"]);
                        exam.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"]);
                        exam.ExamDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ExamDate"]);

                        data.Add(exam);
                    }
                    conn.Close();
                }
            }

            PaggedExamModel paggedExam = new PaggedExamModel()
            {
                ExamModel = data,
                PaggedModel = new PaggedModel()
                {
                    PageIndex = para.PageIndex,
                    TotalPage = totalPage
                }
            };

            return paggedExam;
        }

        public async Task<ExamModel> Get(int id)
        {
            ExamModel exam = await context.Exams.FindAsync(id);
            return exam;
        }

        public async Task Add(ExamModel exam)
        {
            await context.Exams.AddAsync(exam);
            await context.SaveChangesAsync();
        }

        public async Task Update(ExamModel exam)
        {
            context.Attach(exam).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            ExamModel exam = context.Exams.Find(id);
            exam.IsDelete = true;
            exam.IsActive = false;
            await context.SaveChangesAsync();
        }
    }
}
