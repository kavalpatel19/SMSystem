using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
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

        public BaseResponseViewModel<ExamViewModel> GetAllExams()
        {
            var baseResponse = new BaseResponseViewModel<ExamViewModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.GetAsync($"ExamApi/Export").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<ExamViewModel>>(data);
                        return baseResponse;
                    }
                    baseResponse.ResponseCode = (int)response.StatusCode;
                    baseResponse.Message = response.ReasonPhrase;
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Results = new List<ExamViewModel>();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<ExamPaggedViewModel>> GetExams(SearchingParaModel para)
        {
            var baseResponse = new BaseResponseViewModel<ExamPaggedViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    var response = await client.GetAsync($"ExamApi?pageIndex={para.PageIndex}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<ExamPaggedViewModel>>(data);
                        return baseResponse;

                    }
                    baseResponse.ResponseCode = (int)response.StatusCode;
                    baseResponse.Message = response.ReasonPhrase;
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new ExamPaggedViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<ExamViewModel>> GetExam(int id)
        {
            var baseResponse = new BaseResponseViewModel<ExamViewModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    var response = await client.GetAsync($"ExamApi/{id}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<ExamViewModel>>(data);
                        return baseResponse;

                    }
                    baseResponse.ResponseCode = (int)response.StatusCode;
                    baseResponse.Message = response.ReasonPhrase;
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new ExamViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<ExamViewModel>> Add(ExamViewModel exam)
        {
            var baseResponse = new BaseResponseViewModel<ExamViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = await client.PostAsJsonAsync<ExamViewModel>("ExamApi/", exam);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<ExamViewModel>>(data);
                        return baseResponse;

                    }

                    baseResponse.ResponseCode = (int)response.StatusCode;
                    baseResponse.Message = response.ReasonPhrase;
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new ExamViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<ExamViewModel>> Update(ExamViewModel exam)
        {
            var baseResponse = new BaseResponseViewModel<ExamViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = await client.PutAsJsonAsync<ExamViewModel>("ExamApi/", exam);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<ExamViewModel>>(data);

                        return baseResponse;
                    }

                    baseResponse.ResponseCode = (int)response.StatusCode;
                    baseResponse.Message = response.ReasonPhrase;
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new ExamViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<ExamViewModel>> Delete(int id)
        {
            var baseResponse = new BaseResponseViewModel<ExamViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    // To send Delete data request
                    var response = client.DeleteAsync($"ExamApi/{id}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<ExamViewModel>>(data);

                        return baseResponse;
                    }

                    baseResponse.ResponseCode = (int)response.StatusCode;
                    baseResponse.Message = response.ReasonPhrase;
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new ExamViewModel();
                return baseResponse;
            }
        }
    }
}
