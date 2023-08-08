using Microsoft.Data.SqlClient;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Teachers;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace SMSystem_Api.Repository
{
    public class TeacherApiRepository : ITeacherApiRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;

        public TeacherApiRepository(ApplicationDbContext context,IConfiguration configuration)
        {
            this.context = context;
            this.Configuration = configuration;
        }

        public List<TeacherModel> GetAllTeachers()
        {
            List<TeacherModel> teachers = context.Teachers.ToList();
            return teachers;
        }

        public async Task<PaggedTeacherModel> GetAll(SearchingPara para)
        {
            string connaction = Configuration.GetConnectionString("connaction");

            List<TeacherModel> data = new List<TeacherModel>();

            const int pagesize = 5;

            int totalPage = 0;

            using (SqlConnection con = new SqlConnection(connaction))
            {
                SqlCommand cmd = new SqlCommand("TeacherSearchingPaging", con);
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
                        TeacherModel teacher = new TeacherModel();

                        teacher.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        teacher.TeacherId = ds.Tables[0].Rows[i]["TeacherId"].ToString();
                        teacher.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        teacher.Path = ds.Tables[0].Rows[i]["Path"].ToString();
                        teacher.Gender = ds.Tables[0].Rows[i]["Gender"].ToString();
                        teacher.Subject = ds.Tables[0].Rows[i]["Subject"].ToString();
                        teacher.PhoneNo = ds.Tables[0].Rows[i]["PhoneNo"].ToString();
                        teacher.AddressLine1 = ds.Tables[0].Rows[i]["AddressLine1"].ToString();
                        teacher.Country = ds.Tables[0].Rows[i]["Country"].ToString();

                        data.Add(teacher);
                    }
                    conn.Close();
                }
            }

            PaggedTeacherModel paggedTeacher = new PaggedTeacherModel()
            {
                TeacherModel = data,
                PaggedModel = new PaggedModel()
                {
                    PageIndex = para.PageIndex,
                    TotalPage = totalPage
                }
            };

            return paggedTeacher;
        }

        public async Task<TeacherModel> Get(int id)
        {
            TeacherModel teacher = await context.Teachers.FindAsync(id);
            return teacher;
        }

        public async Task Add(TeacherModel teacher)
        {
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();
        }

        public async Task Update(TeacherModel teacher)
        {
            context.Attach(teacher).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            TeacherModel teacher = context.Teachers.Where(x => x.Id == id).FirstOrDefault();
            teacher.IsDelete = true;
            teacher.IsActive = false;
            await context.SaveChangesAsync();
        }
    }
}
