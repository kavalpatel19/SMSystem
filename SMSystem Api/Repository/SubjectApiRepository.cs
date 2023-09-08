using Microsoft.Data.SqlClient;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Subjects;
using System.Data;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Repository.Interfaces;
using Dapper;
using SMSystem_Api.Model.Exam;

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

        public BaseResponseModel<SubjectModel> GetAllSubjects()
        {
            var response = new BaseResponseModel<SubjectModel>();

            try
            {
                var data = context.Subjects.Where(x => x.IsActive).ToList();

                if (data.Count == 0)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Results = new List<SubjectModel>();
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
                response.Results = new List<SubjectModel>();
                return response;
            }
        }

        public async Task<BaseResponseModel<PaggedSubjectModel>> GetAll(SearchingPara para)
        {
            var response = new BaseResponseModel<PaggedSubjectModel>();

            try
            {
                string connaction = Configuration.GetConnectionString("connaction");

                string sp = StoredProcedureHelper.SubjetSearchingPaging;

                var parameters = new DynamicParameters();

                parameters.Add(FieldHelper.SID, para.SId, dbType: DbType.String, size: 50);
                parameters.Add(FieldHelper.Name, para.Name, dbType: DbType.String, size: 50);
                parameters.Add(FieldHelper.Class, para.Class, dbType: DbType.String, size: 50);
                parameters.Add(FieldHelper.PageIndex, para.PageIndex, dbType: DbType.Int32);
                parameters.Add(FieldHelper.PageSize, para.pagesize, dbType: DbType.Int32);
                parameters.Add(FieldHelper.TotalPages, dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = new SqlConnection(connaction))
                {
                    con.Open();
                    var subjects = (await con.QueryAsync<SubjectModel>(sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    var totalPage = parameters.Get<int>(FieldHelper.TotalPages);

                    var paggedSubject = new PaggedSubjectModel()
                    {
                        SubjectModel = subjects,
                        PaggedModel = new PaggedModel()
                        {
                            PageIndex = para.PageIndex,
                            TotalPage = totalPage
                        }
                    };
                    if (subjects.Count == 0)
                    {
                        response.ResponseCode = 404;
                        response.Message = "Data not Found!";
                        response.Result = new PaggedSubjectModel();
                        return response;
                    }
                    response.ResponseCode = 200;
                    response.Result = paggedSubject;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new PaggedSubjectModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<SubjectModel>> Get(int id)
        {
            var response = new BaseResponseModel<SubjectModel>();

            try
            {
                var subject = await context.Subjects.FindAsync(id).ConfigureAwait(false);

                if (subject == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new SubjectModel();
                    return response;
                }

                response.ResponseCode = 200;
                response.Result = subject;
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new SubjectModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<SubjectModel>> Add(SubjectModel subject)
        {
            var response = new BaseResponseModel<SubjectModel>();
            try
            {
                await context.Subjects.AddAsync(subject).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new SubjectModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new SubjectModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<SubjectModel>> Update(SubjectModel subject)
        {
            var response = new BaseResponseModel<SubjectModel>();
            try
            {
                context.Attach(subject).State = EntityState.Modified;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new SubjectModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new SubjectModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<SubjectModel>> Delete(int id)
        {
            var response = new BaseResponseModel<SubjectModel>();
            try
            {
                var subject = context.Subjects.Find(id);
                if (subject == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new SubjectModel();
                    return response;
                }
                subject.IsDelete = true;
                subject.IsActive = false;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new SubjectModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new SubjectModel();
                return response;
            }
        }
    }
}
