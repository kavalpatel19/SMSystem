using Microsoft.Data.SqlClient;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Holiday;
using SMSystem_Api.Repository.Interfaces;
using System.Configuration;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using SMSystem_Api.Model.Fees;
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Migrations;

namespace SMSystem_Api.Repository
{
    public class HolidayApiRepository : IHolidayApiRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;

        public HolidayApiRepository(ApplicationDbContext context,IConfiguration configuration)
        {
            this.context = context;
            this.Configuration = configuration;
        }

        public BaseResponseModel<HolidayModel> GetAllHolidays()
        {
            var response = new BaseResponseModel<HolidayModel>();

            try
            {
                var data = context.Holidays.Where(x => x.IsActive).ToList();

                if (data.Count == 0)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Results = new List<HolidayModel>();
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
                response.Results = new List<HolidayModel>();
                return response;
            }
        }

        public async Task<BaseResponseModel<PaggedHolidayModel>> GetAll(SearchingPara para)
        {
            var response = new BaseResponseModel<PaggedHolidayModel>();

            try
            {
                string connaction = Configuration.GetConnectionString("connaction");

                string sp = StoredProcedureHelper.HolidayPaging;

                var parameters = new DynamicParameters();

                parameters.Add(FieldHelper.PageIndex, para.PageIndex, dbType: DbType.Int32);
                parameters.Add(FieldHelper.PageSize, para.pagesize, dbType: DbType.Int32);
                parameters.Add(FieldHelper.TotalPages, dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = new SqlConnection(connaction))
                {
                    con.Open();
                    var holidays = (await con.QueryAsync<HolidayModel>(sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    var totalPage = parameters.Get<int>(FieldHelper.TotalPages);

                    var paggedHolidayModel = new PaggedHolidayModel()
                    {
                        HolidayModel = holidays,
                        PaggedModel = new PaggedModel()
                        {
                            PageIndex = para.PageIndex,
                            TotalPage = totalPage
                        }
                    };
                    if (holidays.Count == 0)
                    {
                        response.ResponseCode = 404;
                        response.Message = "Data not Found!";
                        response.Result = new PaggedHolidayModel();
                        return response;
                    }
                    response.ResponseCode = 200;
                    response.Result = paggedHolidayModel;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new PaggedHolidayModel();
                return response;
            }
        }
    

        public async Task<BaseResponseModel<HolidayModel>> Add(HolidayModel holiday)
        {
            var response = new BaseResponseModel<HolidayModel>();
            try
            {
                await context.Holidays.AddAsync(holiday).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new HolidayModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new HolidayModel();
                return response;
            }
        }
    }
}
