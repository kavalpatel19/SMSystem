using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Holiday;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Repository
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly IConfiguration configuration;

        public HolidayRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<HolidayViewModel> GetAllHolidays()
        {
            var holidays = new List<HolidayViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.GetAsync($"HolidayApi/Export").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    holidays = JsonConvert.DeserializeObject<List<HolidayViewModel>>(data);
                }

                return holidays;
            }
        }

        public async Task<HolidayPaggedViewModel> GetHolidays(SearchingParaModel para)
        {
            var holidays = new HolidayPaggedViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                para.SId = string.IsNullOrEmpty(para.SId) ? string.Empty : para.SId;
                para.Name = string.IsNullOrEmpty(para.Name) ? string.Empty : para.Name;
                para.Year = string.IsNullOrEmpty(para.Year) ? string.Empty : para.Year;

                var response = await client.GetAsync($"HolidayApi?pageIndex={para.PageIndex}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    holidays = JsonConvert.DeserializeObject<HolidayPaggedViewModel>(data);
                }

                return holidays;
            }
        }

        public async Task<bool> Add(HolidayViewModel holiday)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.PostAsJsonAsync<HolidayViewModel>("HolidayApi/", holiday).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
