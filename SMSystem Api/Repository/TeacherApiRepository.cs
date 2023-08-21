using Microsoft.Data.SqlClient;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Teachers;
using System.Data;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Repository.Interfaces;
using Dapper;
using SMSystem_Api.Model.Department;

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
            var teachers = context.Teachers.Where(x => x.IsActive).ToList();
            return teachers;
        }

        public async Task<PaggedTeacherModel> GetAll(SearchingPara para)
        {
            string connaction = Configuration.GetConnectionString("connaction");

            string sp = StoredProcedureHelper.TeacherSearchingPaging;

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
                var teachers = (await con.QueryAsync<TeacherModel>( sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                var totalPage = parameters.Get<int>(FieldHelper.TotalPages);

                var paggedTeacher = new PaggedTeacherModel()
                {
                    TeacherModel = teachers,
                    PaggedModel = new PaggedModel()
                    {
                        PageIndex = para.PageIndex,
                        TotalPage = totalPage
                    }
                };

                return paggedTeacher;
            }
        }

        public async Task<TeacherModel> Get(int id)
        {
            var teacher = await context.Teachers.FindAsync(id).ConfigureAwait(false);
            return teacher;
        }

        public async Task Add(TeacherModel teacher)
        {
            await context.Teachers.AddAsync(teacher).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(TeacherModel teacher)
        {
            context.Attach(teacher).State = EntityState.Modified;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            var teacher = context.Teachers.Where(x => x.Id == id).FirstOrDefault();
            teacher.IsDelete = true;
            teacher.IsActive = false;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
