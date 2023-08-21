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

        public List<FeesModel> GetAllFees()
        {
            var Data = context.Fees.Where(x => x.IsActive).ToList();
            return Data;
        }

        public async Task<PaggedFeesModel> GetAll(SearchingPara para)
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
                var fees = (await con.QueryAsync<FeesModel>( sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

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

                return paggedFees;
            }
        }

        public async Task<FeesModel> Get(int id)
        {
            var fee = await context.Fees.FindAsync(id).ConfigureAwait(false);
            return fee;
        }

        public async Task Add(FeesModel fee)
        {
            await context.Fees.AddAsync(fee).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(FeesModel fee)
        {
            context.Attach(fee).State = EntityState.Modified;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            var fee = context.Fees.Find(id);
            fee.IsDelete = true;
            fee.IsActive = false;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
