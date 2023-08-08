using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Subject;

namespace SMSystem.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IConfiguration configuration;

        public SubjectRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<SubjectViewModel> GetAllSubjects()
        {
            var subjects = new List<SubjectViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.GetAsync($"SubjectApi/Export").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    subjects = JsonConvert.DeserializeObject<List<SubjectViewModel>>(data);
                }

                return subjects;
            }
        }

        public async Task<SubjectPaggedViewModel> GetSubjects(SearchingParaModel para)
        {
            SubjectPaggedViewModel subjects = new SubjectPaggedViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                para.SId = string.IsNullOrEmpty(para.SId) ? string.Empty : para.SId;
                para.Name = string.IsNullOrEmpty(para.Name) ? string.Empty : para.Name;
                para.Class = string.IsNullOrEmpty(para.Class) ? string.Empty : para.Class;

                var response = await client.GetAsync($"SubjectApi?sid={para.SId}&name={para.Name}&year={para.Class}&pageIndex={para.PageIndex}");

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    subjects = JsonConvert.DeserializeObject<SubjectPaggedViewModel>(data);
                }

                return subjects;
            }
        }

        public async Task<SubjectViewModel> GetSubject(int id)
        {
            SubjectViewModel subject = new SubjectViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                var response = await client.GetAsync($"SubjectApi/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    subject = JsonConvert.DeserializeObject<SubjectViewModel>(data);
                }

                return subject;
            }
        }

        public async Task<bool> Add(SubjectViewModel subject)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.PostAsJsonAsync<SubjectViewModel>("SubjectApi/", subject).Result;

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

        public async Task<bool> Update(SubjectViewModel subject)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.PutAsJsonAsync<SubjectViewModel>("SubjectApi/", subject).Result;

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
                var response = client.DeleteAsync($"SubjectApi/{id}").Result;

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
