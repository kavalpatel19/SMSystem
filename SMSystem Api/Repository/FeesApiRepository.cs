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
using SMSystem_Api.Model.Subjects;
using SMSystem_Api.Repository.Interfaces;
using System.Data;

namespace SMSystem_Api.Repository
{
    public class FeesApiRepository : IFeesApiRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;

        public FeesApiRepository(ApplicationDbContext context,IConfiguration configuration)
        {
            this.context = context;
            this.Configuration = configuration;
        }

        public BaseResponseModel<FeesModel> GetAllFees()
        {
            var response = new BaseResponseModel<FeesModel>();

            try
            {
                var data = context.Fees.Where(x => x.IsActive).ToList();

                if (data.Count == 0)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Results = new List<FeesModel>();
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
                response.Results = new List<FeesModel>();
                return response;
            }
        }

        public async Task<BaseResponseModel<PaggedFeesModel>> GetAll(SearchingPara para)
        {
            var response = new BaseResponseModel<PaggedFeesModel>();

            try
            {
                string connaction = Configuration.GetConnectionString("connaction");

                string sp = StoredProcedureHelper.FeesPaging;

                var parameters = new DynamicParameters();

                parameters.Add(FieldHelper.PageIndex, para.PageIndex, dbType: DbType.Int32);
                parameters.Add(FieldHelper.PageSize, para.pagesize, dbType: DbType.Int32);
                parameters.Add(FieldHelper.TotalPages, dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = new SqlConnection(connaction))
                {
                    con.Open();
                    var fees = (await con.QueryAsync<FeesModel>(sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    var totalPage = parameters.Get<int>(FieldHelper.TotalPages);

                    var paggedFees = new PaggedFeesModel()
                    {
                        FeesModel = fees,
                        PaggedModel = new PaggedModel()
                        {
                            PageIndex = para.PageIndex,
                            TotalPage = totalPage
                        }
                    };
                    if (fees.Count == 0)
                    {
                        response.ResponseCode = 404;
                        response.Message = "Data not Found!";
                        response.Result = new PaggedFeesModel();
                        return response;
                    }
                    response.ResponseCode = 200;
                    response.Result = paggedFees;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new PaggedFeesModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<FeesModel>> Get(int id)
        {
            var response = new BaseResponseModel<FeesModel>();

            try
            {
                var fee = await context.Fees.FindAsync(id).ConfigureAwait(false);

                if (fee == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new FeesModel();
                    return response;
                }

                response.ResponseCode = 200;
                response.Result = fee;
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new FeesModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<FeesModel>> Add(FeesModel fee)
        {
            var response = new BaseResponseModel<FeesModel>();
            try
            {
                await context.Fees.AddAsync(fee).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new FeesModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new FeesModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<FeesModel>> Update(FeesModel fee)
        {
            var response = new BaseResponseModel<FeesModel>();
            try
            {
                context.Attach(fee).State = EntityState.Modified;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new FeesModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new FeesModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<FeesModel>> Delete(int id)
        {
            var response = new BaseResponseModel<FeesModel>();
            try
            {
                var fee = context.Fees.Find(id);
                if (fee == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new FeesModel();
                    return response;
                }
                fee.IsDelete = true;
                fee.IsActive = false;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new FeesModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new FeesModel();
                return response;
            }
        }
    }
}
