using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models.Exam;
using SMSystem.Models.Fees;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly IConfiguration configuration;

        public ExamRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<ExamViewModel> GetAllExams()
        {
            var exams = new List<ExamViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.GetAsync($"ExamApi/Export").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    exams = JsonConvert.DeserializeObject<List<ExamViewModel>>(data);
                }

                return exams;
            }
        }

        public async Task<ExamPaggedViewModel> GetExams(SearchingParaModel para)
        {
            var exams = new ExamPaggedViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                var response = await client.GetAsync($"ExamApi?pageIndex={para.PageIndex}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    exams = JsonConvert.DeserializeObject<ExamPaggedViewModel>(data);
                }

                return exams;
            }
        }

        public async Task<ExamViewModel> GetExam(int id)
        {
            var exam = new ExamViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                var response = await client.GetAsync($"ExamApi/{id}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    exam = JsonConvert.DeserializeObject<ExamViewModel>(data);
                }

                return exam;
            }
        }

        public async Task<bool> Add(ExamViewModel exam)
        {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PostAsJsonAsync<ExamViewModel>("ExamApi/", exam).Result;

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

        public async Task<bool> Update(ExamViewModel exam)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.PutAsJsonAsync<ExamViewModel>("ExamApi/", exam).Result;

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
                var response = client.DeleteAsync($"ExamApi/{id}").Result;

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
