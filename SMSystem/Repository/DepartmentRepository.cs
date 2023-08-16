using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Students;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IConfiguration configuration;

        public DepartmentRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<DepartmentViewModel> GetAllDepartments()
        {
            var departments = new List<DepartmentViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.GetAsync($"DepartmentApi/Export").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    departments = JsonConvert.DeserializeObject<List<DepartmentViewModel>>(data);
                }

                return departments;
            }
        }

        public async Task<DepartmentPaggedViewModel> GetDepartmnets(SearchingParaModel para)
        {
            var departments = new DepartmentPaggedViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                para.SId = string.IsNullOrEmpty(para.SId) ? string.Empty : para.SId;
                para.Name = string.IsNullOrEmpty(para.Name) ? string.Empty : para.Name;
                para.Year = string.IsNullOrEmpty(para.Year) ? string.Empty : para.Year;

                var response = await client.GetAsync($"DepartmentApi?sid={para.SId}&name={para.Name}&year={para.Year}&pageIndex={para.PageIndex}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    departments = JsonConvert.DeserializeObject<DepartmentPaggedViewModel>(data);
                }

                return departments;
            }
        }

        public async Task<DepartmentViewModel> GetDepartment(int id)
        {
            var department = new DepartmentViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                var response = await client.GetAsync($"DepartmentApi/{id}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    department = JsonConvert.DeserializeObject<DepartmentViewModel>(data);
                }

                return department;
            }
        }

        public async Task<bool> Add(DepartmentViewModel department)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.PostAsJsonAsync<DepartmentViewModel>("DepartmentApi/", department).Result;

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

        public async Task<bool> Update(DepartmentViewModel department)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.PutAsJsonAsync<DepartmentViewModel>("DepartmentApi/", department).Result;

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
                var response = client.DeleteAsync($"DepartmentApi/{id}").Result;

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
