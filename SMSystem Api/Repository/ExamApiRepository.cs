using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Migrations;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
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

        public BaseResponseModel<ExamModel> GetAllExams()
        {
            var response = new BaseResponseModel<ExamModel>();
            try
            {
                var data = context.Exams.Where(x => x.IsActive).ToList();

                if (data.Count == 0)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Results = new List<ExamModel>();
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
                response.Results = new List<ExamModel>();
                return response;
            }
        }

        public async Task<BaseResponseModel<PaggedExamModel>> GetAll(SearchingPara para)
        {
            var response = new BaseResponseModel<PaggedExamModel>();
            try
            {
                string connaction = Configuration.GetConnectionString("connaction");

                string sp = StoredProcedureHelper.ExamPaging;

                var parameters = new DynamicParameters();

                parameters.Add(FieldHelper.PageIndex, para.PageIndex, dbType: DbType.Int32);
                parameters.Add(FieldHelper.PageSize, para.pagesize, dbType: DbType.Int32);
                parameters.Add(FieldHelper.TotalPages, dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = new SqlConnection(connaction))
                {
                    con.Open();
                    var exams = (await con.QueryAsync<ExamModel>(sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    var totalPage = parameters.Get<int>(FieldHelper.TotalPages);

                    var paggedExam = new PaggedExamModel()
                    {
                        ExamModel = exams,
                        PaggedModel = new PaggedModel()
                        {
                            PageIndex = para.PageIndex,
                            TotalPage = totalPage
                        }
                    };
                    if (exams.Count == 0)
                    {
                        response.ResponseCode = 404;
                        response.Message = "Data not Found!";
                        response.Result = new PaggedExamModel();
                        return response;
                    }
                    response.ResponseCode = 200;
                    response.Result = paggedExam;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new PaggedExamModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<ExamModel>> Get(int id)
        {
            var response = new BaseResponseModel<ExamModel>();
            try
            {
                var exam = await context.Exams.FindAsync(id).ConfigureAwait(false);

                if (exam == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new ExamModel();
                    return response;
                }

                response.ResponseCode = 200;
                response.Result = exam;
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new ExamModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<ExamModel>> Add(ExamModel exam)
        {
            var response = new BaseResponseModel<ExamModel>();
            try
            {
                await context.Exams.AddAsync(exam).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new ExamModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new ExamModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<ExamModel>> Update(ExamModel exam)
        {
            var response = new BaseResponseModel<ExamModel>();
            try
            {
                context.Attach(exam).State = EntityState.Modified;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new ExamModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new ExamModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<ExamModel>> Delete(int id)
        {
            var response = new BaseResponseModel<ExamModel>();
            try
            {
                var exam = context.Exams.Find(id);
                if(exam == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new ExamModel();
                    return response;
                }
                exam.IsDelete = true;
                exam.IsActive = false;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new ExamModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new ExamModel();
                return response;
            }
        }
    }
}
