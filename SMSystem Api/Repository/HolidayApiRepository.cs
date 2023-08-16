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

            var data = new List<HolidayModel>();

            const int pagesize = 5;

            int totalPage = 0;

            using (var con = new SqlConnection(connaction))
            {
                var cmd = new SqlCommand("HolidayPaging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@PageIndex", SqlDbType.VarChar).Value = para.PageIndex;
                cmd.Parameters.Add("@PageSize", SqlDbType.VarChar).Value = pagesize;
                cmd.Parameters.Add("@TotalPages", SqlDbType.Int, 4);
                cmd.Parameters["@TotalPages"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                con.Close();

                totalPage = Convert.ToInt32(cmd.Parameters["@TotalPages"].Value);

                using (var conn = new SqlConnection(connaction))
                {
                    var ds = new DataSet();

                    conn.Open();
                    var rdr = new SqlDataAdapter();
                    rdr.SelectCommand = cmd;
                    rdr.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var holiday = new HolidayModel();

                        holiday.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        holiday.HolidayId = ds.Tables[0].Rows[i]["HolidayId"].ToString();
                        holiday.HolidayName = ds.Tables[0].Rows[i]["HolidayName"].ToString();
                        holiday.HolidayType = ds.Tables[0].Rows[i]["HolidayType"].ToString();
                        holiday.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"]);
                        holiday.EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"]);

                        data.Add(holiday);
                    }
                    conn.Close();
                }
            }

            var paggedHolidayModel = new PaggedHolidayModel()
            {
                HolidayModel = data,
                PaggedModel = new PaggedModel()
                {
                    PageIndex = para.PageIndex,
                    TotalPage = totalPage
                }
            };

            return paggedHolidayModel;
        }

        public async Task Add(HolidayModel holiday)
        {
            await context.Holidays.AddAsync(holiday).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
