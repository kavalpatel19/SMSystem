using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
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
            var student = context.Students.Where(x => x.IsActive).ToList();
            return student;
        }

        public async Task<PaggedStudentModel> GetAll(SearchingPara para)
        {
            string connaction = Configuration.GetConnectionString("connaction");

            string sp = StoredProcedureHelper.StudentSearchingPaging;

            var parameters = new DynamicParameters();

            parameters.Add(FieldHelper.SID, para.SId, dbType: DbType.String, size: 50);
            parameters.Add(FieldHelper.Name, para.Name, dbType: DbType.String, size: 50);
            parameters.Add(FieldHelper.Phone, para.Phone, dbType: DbType.String, size: 50);
            parameters.Add(FieldHelper.PageIndex, para.PageIndex, dbType: DbType.Int32);
            parameters.Add(FieldHelper.PageSize, para.pagesize, dbType: DbType.Int32);
            parameters.Add(FieldHelper.TotalPages, dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var con = new SqlConnection(connaction))
            {
                con.Open();
                var students = (await con.QueryAsync<StudentModel>( sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                var totalPage = parameters.Get<int>(FieldHelper.TotalPages);

                var paggedStudent = new PaggedStudentModel()
                {
                    StudentModel = students,
                    PaggedModel = new PaggedModel()
                    {
                        PageIndex = para.PageIndex,
                        TotalPage = totalPage
                    }
                };

                return paggedStudent;
            }
        }
        public async Task<StudentModel> Get(int id)
        {
            var student = await context.Students.FindAsync(id).ConfigureAwait(false);
            return student;
        } 

        public async Task Add(StudentModel student)
        {
            await context.Students.AddAsync(student).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(StudentModel student)
        {
            context.Attach(student).State = EntityState.Modified;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            var student = context.Students.Where(x => x.Id == id).FirstOrDefault();
            student.IsDelete = true;
            student.IsActive = false;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

    }
}
