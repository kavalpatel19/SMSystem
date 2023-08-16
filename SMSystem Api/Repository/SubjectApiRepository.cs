using Microsoft.Data.SqlClient;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Subjects;
using System.Data;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Repository.Interfaces;

namespace SMSystem_Api.Repository
{
    public class SubjectApiRepository : ISubjectApiRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;

        public SubjectApiRepository(ApplicationDbContext context,IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
        }

        public List<SubjectModel> GetAllSubjects()
        {
            var Data = context.Subjects.Where(x => x.IsActive).ToList();
            return Data;
        }

        public async Task<PaggedSubjectModel> GetAll(SearchingPara para)
        {
            string connaction = Configuration.GetConnectionString("connaction");

            var data = new List<SubjectModel>();

            const int pagesize = 5;

            int totalPage = 0;

            using (var con = new SqlConnection(connaction))
            {
                var cmd = new SqlCommand("SubjetSearchingPaging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@SID", SqlDbType.VarChar).Value = para.SId;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = para.Name;
                cmd.Parameters.Add("@Class", SqlDbType.VarChar).Value = para.Class;
                cmd.Parameters.Add("@PageIndex", SqlDbType.VarChar).Value = para.PageIndex;
                cmd.Parameters.Add("@PageSize", SqlDbType.VarChar).Value = pagesize;
                cmd.Parameters.Add("@TotalPages", SqlDbType.Int, 4);
                cmd.Parameters["@TotalPages"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                con.Close();

                totalPage = Convert.ToInt32(cmd.Parameters["@TotalPages"].Value);

                using (var conn = new SqlConnection(connaction))
                {
                    var ds = new DataSet();

                    conn.Open();
                    var rdr = new SqlDataAdapter();
                    rdr.SelectCommand = cmd;
                    rdr.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var subject = new SubjectModel();

                        subject.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        subject.SubjectId = ds.Tables[0].Rows[i]["SubjectId"].ToString();
                        subject.SubjectName = ds.Tables[0].Rows[i]["SubjectName"].ToString();
                        subject.Class = ds.Tables[0].Rows[i]["Class"].ToString();

                        data.Add(subject);
                    }
                    conn.Close();
                }
            }

            var paggedSubject = new PaggedSubjectModel()
            {
                SubjectModel = data,
                PaggedModel = new PaggedModel()
                {
                    PageIndex = para.PageIndex,
                    TotalPage = totalPage
                }
            };

            return paggedSubject;
        }

        public async Task<SubjectModel> Get(int id)
        {
            var subject = await context.Subjects.FindAsync(id).ConfigureAwait(false);
            return subject;
        }

        public async Task Add(SubjectModel subject)
        {
            await context.Subjects.AddAsync(subject).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(SubjectModel subject)
        {
            context.Attach(subject).State = EntityState.Modified;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            var subject = context.Subjects.Find(id);
            subject.IsDelete = true;
            subject.IsActive = false;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
