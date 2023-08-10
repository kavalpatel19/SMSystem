using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Repository.Interfaces;
using System.Configuration;
using System.Data;
using System.Drawing.Text;

namespace SMSystem_Api.Repository
{
    public class StudentApiRepository : IStudentApiRepository
    {
        private readonly ApplicationDbContext context;
        public IConfiguration Configuration;

        public StudentApiRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
        }

        public List<StudentModel> GetAllStudents()
        {
            List<StudentModel> student = context.Students.Where(x => x.IsActive).ToList();
            return student;
        }

        public async Task<PaggedStudentModel> GetAll(SearchingPara para)
        {
            string connaction = Configuration.GetConnectionString("connaction");

            List<StudentModel> data = new List<StudentModel>();

            const int pagesize = 5;

            int totalPage = 0;

            using (SqlConnection con = new SqlConnection(connaction))
            {
                SqlCommand cmd = new SqlCommand("StudentSearchingPaging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@SID", SqlDbType.VarChar).Value = para.SId;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = para.Name;
                cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = para.Phone;
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
                        StudentModel student = new StudentModel();

                        student.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        student.StudentId = ds.Tables[0].Rows[i]["StudentId"].ToString();
                        student.FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        student.LastName = ds.Tables[0].Rows[i]["LastName"].ToString();
                        student.Path = ds.Tables[0].Rows[i]["Path"].ToString();
                        student.ParentName = ds.Tables[0].Rows[i]["ParentName"].ToString();
                        student.DateOfBirth = Convert.ToDateTime(ds.Tables[0].Rows[i]["DateOfBirth"]);
                        student.Class = ds.Tables[0].Rows[i]["Class"].ToString();
                        student.Phone = ds.Tables[0].Rows[i]["Phone"].ToString();
                        student.Address = ds.Tables[0].Rows[i]["Address"].ToString();

                        data.Add(student);
                    }
                    conn.Close();
                }
            }

            PaggedStudentModel paggedStudent = new PaggedStudentModel()
            {
                StudentModel = data,
                PaggedModel = new PaggedModel()
                {
                    PageIndex = para.PageIndex,
                    TotalPage = totalPage
                }
            };

            return paggedStudent;
        }
        public async Task<StudentModel> Get(int id)
        {
            StudentModel student = await context.Students.FindAsync(id);
            return student;
        } 

        public async Task Add(StudentModel student)
        {
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();
        }

        public async Task Update(StudentModel student)
        {
            context.Attach(student).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            StudentModel student = context.Students.Where(x => x.Id == id).FirstOrDefault();
            student.IsDelete = true;
            student.IsActive = false;
            await context.SaveChangesAsync();
        }

    }
}
