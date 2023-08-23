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
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Migrations;

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

        public BaseResponseModel<TeacherModel> GetAllTeachers()
        {
            var response = new BaseResponseModel<TeacherModel>();
            try
            {
                var data = context.Teachers.Where(x => x.IsActive).ToList();

                if (data.Count == 0)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Results = new List<TeacherModel>();
                    return response;
                }
                response.ResponseCode = 200;
                response.Results = data;
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Results = new List<TeacherModel>();
                return response;
            }
        }

        public async Task<BaseResponseModel<PaggedTeacherModel>> GetAll(SearchingPara para)
        {
            var response = new BaseResponseModel<PaggedTeacherModel>();
            try
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
                    var teachers = (await con.QueryAsync<TeacherModel>(sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

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
                    if (teachers.Count == 0)
                    {
                        response.ResponseCode = 404;
                        response.Message = "Data not Found!";
                        response.Result = new PaggedTeacherModel();
                        return response;
                    }
                    response.ResponseCode = 200;
                    response.Result = paggedTeacher;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new PaggedTeacherModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<TeacherModel>> Get(int id)
        {
            var response = new BaseResponseModel<TeacherModel>();
            try
            {
                var teacher = await context.Teachers.FindAsync(id).ConfigureAwait(false);

                if (teacher == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new TeacherModel();
                    return response;
                }

                response.ResponseCode = 200;
                response.Result = teacher;
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new TeacherModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<TeacherModel>> Add(TeacherModel teacher)
        {
            var response = new BaseResponseModel<TeacherModel>();
            try
            {
                await context.Teachers.AddAsync(teacher).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new TeacherModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new TeacherModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<TeacherModel>> Update(TeacherModel teacher)
        {
            var response = new BaseResponseModel<TeacherModel>();
            try
            {
                context.Attach(teacher).State = EntityState.Modified;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new TeacherModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new TeacherModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<TeacherModel>> Delete(int id)
        {
            var response = new BaseResponseModel<TeacherModel>();
            try
            {
                var teacher = context.Teachers.Find(id);
                if (teacher == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new TeacherModel();
                    return response;
                }
                teacher.IsDelete = true;
                teacher.IsActive = false;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new TeacherModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new TeacherModel();
                return response;
            }
        }
    }
}
