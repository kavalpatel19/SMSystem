using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Migrations;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Exam;
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

        public BaseResponseModel<StudentModel> GetAllStudents()
        {
            var response = new BaseResponseModel<StudentModel>();

            try
            {
                var data = context.Students.Where(x => x.IsActive).ToList();

                if (data.Count == 0)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Results = new List<StudentModel>();
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
                response.Results = new List<StudentModel>();
                return response;
            }
        }

        public async Task<BaseResponseModel<PaggedStudentModel>> GetAll(SearchingPara para)
        {
            var response = new BaseResponseModel<PaggedStudentModel>();

            try
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
                    var students = (await con.QueryAsync<StudentModel>(sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

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
                    if (students.Count == 0)
                    {
                        response.ResponseCode = 404;
                        response.Message = "Data not Found!";
                        response.Result = new PaggedStudentModel();
                        return response;
                    }
                    response.ResponseCode = 200;
                    response.Result = paggedStudent;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new PaggedStudentModel();
                return response;
            }
        }
        public async Task<BaseResponseModel<StudentModel>> Get(int id)
        {
            var response = new BaseResponseModel<StudentModel>();

            try
            {
                var student = await context.Students.FindAsync(id).ConfigureAwait(false);

                if (student == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new StudentModel();
                    return response;
                }

                response.ResponseCode = 200;
                response.Result = student;
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new StudentModel();
                return response;
            }
        } 

        public async Task<BaseResponseModel<StudentModel>> Add(StudentModel student)
        {
            var response = new BaseResponseModel<StudentModel>();
            try
            {
                await context.Students.AddAsync(student).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new StudentModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new StudentModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<StudentModel>> Update(StudentModel student)
        {
            var response = new BaseResponseModel<StudentModel>();
            try
            {
                context.Attach(student).State = EntityState.Modified;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new StudentModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new StudentModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<StudentModel>> Delete(int id)
        {
            var response = new BaseResponseModel<StudentModel>();
            try
            {
                var student = context.Students.Find(id);
                if (student == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new StudentModel();
                    return response;
                }
                student.IsDelete = true;
                student.IsActive = false;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new StudentModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new StudentModel();
                return response;
            }
            
        }

    }
}
