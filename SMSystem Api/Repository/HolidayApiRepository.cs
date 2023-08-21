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

        public List<HolidayModel> GetAllHolidays()
        {
            var Data = context.Holidays.Where(x => x.IsActive).ToList();
            return Data;
        }

        public async Task<PaggedHolidayModel> GetAll(SearchingPara para)
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
                var holidays = (await con.QueryAsync<HolidayModel>( sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

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

                return paggedHolidayModel;
            }
        }
    

        public async Task Add(HolidayModel holiday)
        {
            await context.Holidays.AddAsync(holiday).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
