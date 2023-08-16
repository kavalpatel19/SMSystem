using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Fees;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Repository
{
    public class FeesRepository : IFeesRepository
    {
        private readonly IConfiguration configuration;

        public FeesRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<FeesViewModel> GetAllFees()
        {
            var fees = new List<FeesViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.GetAsync($"FeesApi/Export").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    fees = JsonConvert.DeserializeObject<List<FeesViewModel>>(data);
                }

                return fees;
            }
        }

        public async Task<FeesPaggedViewModel> GetFees(SearchingParaModel para)
        {
            var Fees = new FeesPaggedViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                var response = await client.GetAsync($"FeesApi?pageIndex={para.PageIndex}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    Fees = JsonConvert.DeserializeObject<FeesPaggedViewModel>(data);
                }

                return Fees;
            }
        }

        public async Task<FeesViewModel> GetFee(int id)
        {
            var fee = new FeesViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                var response = await client.GetAsync($"FeesApi/{id}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    fee = JsonConvert.DeserializeObject<FeesViewModel>(data);
                }

                return fee;
            }
        }

        public async Task<bool> Add(FeesViewModel fee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.PostAsJsonAsync<FeesViewModel>("FeesApi/", fee).Result;

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

        public async Task<bool> Update(FeesViewModel fee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.PutAsJsonAsync<FeesViewModel>("FeesApi/", fee).Result;

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

        public async Task<bool> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                // To send Delete data request
                var response = client.DeleteAsync($"FeesApi/{id}").Result;

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
