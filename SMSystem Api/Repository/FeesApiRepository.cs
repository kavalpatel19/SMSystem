using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Migrations;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
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

            List<FeesModel> data = new List<FeesModel>();

            const int pagesize = 5;

            int totalPage = 0;

            using (SqlConnection con = new SqlConnection(connaction))
            {
                SqlCommand cmd = new SqlCommand("FeesPaging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@PageIndex", SqlDbType.VarChar).Value = para.PageIndex;
                cmd.Parameters.Add("@PageSize", SqlDbType.VarChar).Value = pagesize;
                cmd.Parameters.Add("@TotalPages", SqlDbType.Int, 4);
                cmd.Parameters["@TotalPages"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                con.Close();

                totalPage = Convert.ToInt32(cmd.Parameters["@TotalPages"].Value);

                using (SqlConnection conn = new SqlConnection(connaction))
                {
                    DataSet ds = new DataSet();

                    conn.Open();
                    SqlDataAdapter rdr = new SqlDataAdapter();
                    rdr.SelectCommand = cmd;
                    rdr.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FeesModel fee = new FeesModel();

                        fee.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        fee.FeesId = ds.Tables[0].Rows[i]["FeesId"].ToString();
                        fee.FeesType = ds.Tables[0].Rows[i]["FeesType"].ToString();
                        fee.Class = ds.Tables[0].Rows[i]["Class"].ToString();
                        fee.FeesAmount = Convert.ToInt32(ds.Tables[0].Rows[i]["FeesAmount"]);
                        fee.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"]);
                        fee.EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"]);

                        data.Add(fee);
                    }
                    conn.Close();
                }
            }

            PaggedFeesModel paggedFees = new PaggedFeesModel()
            {
                FeesModel = data,
                PaggedModel = new PaggedModel()
                {
                    PageIndex = para.PageIndex,
                    TotalPage = totalPage
                }
            };

            return paggedFees;
        }

        public async Task<FeesModel> Get(int id)
        {
            FeesModel fee = await context.Fees.FindAsync(id);
            return fee;
        }

        public async Task Add(FeesModel fee)
        {
            await context.Fees.AddAsync(fee);
            await context.SaveChangesAsync();
        }

        public async Task Update(FeesModel fee)
        {
            context.Attach(fee).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            FeesModel fee = context.Fees.Find(id);
            fee.IsDelete = true;
            fee.IsActive = false;
            await context.SaveChangesAsync();
        }
    }
}
