using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Migrations;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Repository.Interfaces;
using System.Data;

namespace SMSystem_Api.Repository
{
    public class DepartmentApiRepository : IDepartmentApiRepository
    {
        private readonly ApplicationDbContext context;
        public IConfiguration Configuration;

        public DepartmentApiRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
        }

        public List<DepartmentModel> GetAllDepartments()
        {
            var Data = context.Departments.Where(x => x.IsActive).ToList();
            return Data;
        }

        public async Task<PaggedDepartmentModel> GetAll(SearchingPara para)
        {
            string connaction = Configuration.GetConnectionString("connaction");

            List<DepartmentModel> data = new List<DepartmentModel>();

            const int pagesize = 5;

            int totalPage = 0;

            using (SqlConnection con = new SqlConnection(connaction))
            {
                SqlCommand cmd = new SqlCommand("DepartmentSearchingPaging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@SID", SqlDbType.VarChar).Value = para.SId;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = para.Name;
                cmd.Parameters.Add("@Year", SqlDbType.VarChar).Value = para.Year;
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
                        DepartmentModel department = new DepartmentModel();

                        department.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        department.DepartmentId = ds.Tables[0].Rows[i]["DepartmentId"].ToString();
                        department.DepartmentName = ds.Tables[0].Rows[i]["DepartmentName"].ToString();
                        department.HeadOfDepartment = ds.Tables[0].Rows[i]["HeadOfDepartment"].ToString();
                        department.StartedYear = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartedYear"]);
                        department.NoOfStudents = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudents"]);

                        data.Add(department);
                    }
                    conn.Close();
                }
            }

            PaggedDepartmentModel paggedDepartment = new PaggedDepartmentModel()
            {
                DepartmentModel = data,
                PaggedModel = new PaggedModel()
                {
                    PageIndex = para.PageIndex,
                    TotalPage = totalPage
                }
            };

            return paggedDepartment;
        }
        public async Task<DepartmentModel> Get(int id)
        {
            DepartmentModel department = await context.Departments.FindAsync(id);
            return department;
        }

        public async Task Add(DepartmentModel department)
        {
            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();
        }

        public async Task Update(DepartmentModel department)
        {
            context.Attach(department).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            DepartmentModel department = context.Departments.Find(id);
            department.IsDelete = true;
            department.IsActive = false;
            await context.SaveChangesAsync();
        }
    }
}
