using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Exam;
using SMSystem.Models.Subject;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IConfiguration configuration;

        public SubjectRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public BaseResponseViewModel<SubjectViewModel> GetAllSubjects()
        {
            var baseResponse = new BaseResponseViewModel<SubjectViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.GetAsync($"SubjectApi/Export").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<SubjectViewModel>>(data);
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
                baseResponse.Results = new List<SubjectViewModel>();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<SubjectPaggedViewModel>> GetSubjects(SearchingParaModel para)
        {
            var baseResponse = new BaseResponseViewModel<SubjectPaggedViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    para.SId = string.IsNullOrEmpty(para.SId) ? string.Empty : para.SId;
                    para.Name = string.IsNullOrEmpty(para.Name) ? string.Empty : para.Name;
                    para.Class = string.IsNullOrEmpty(para.Class) ? string.Empty : para.Class;

                    var response = await client.GetAsync($"SubjectApi?sid={para.SId}&name={para.Name}&clas={para.Class}&pageIndex={para.PageIndex}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<SubjectPaggedViewModel>>(data);
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
                baseResponse.Result = new SubjectPaggedViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<SubjectViewModel>> GetSubject(int id)
        {
            var baseResponse = new BaseResponseViewModel<SubjectViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    var response = await client.GetAsync($"SubjectApi/{id}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<SubjectViewModel>>(data);
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
                baseResponse.Result = new SubjectViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<SubjectViewModel>> Add(SubjectViewModel subject)
        {
            var baseResponse = new BaseResponseViewModel<SubjectViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PostAsJsonAsync<SubjectViewModel>("SubjectApi/", subject).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<SubjectViewModel>>(data);
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
                baseResponse.Result = new SubjectViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<SubjectViewModel>> Update(SubjectViewModel subject)
        {
            var baseResponse = new BaseResponseViewModel<SubjectViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PutAsJsonAsync<SubjectViewModel>("SubjectApi/", subject).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<SubjectViewModel>>(data);

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
                baseResponse.Result = new SubjectViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<SubjectViewModel>> Delete(int id)
        {
            var baseResponse = new BaseResponseViewModel<SubjectViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    // To send Delete data request
                    var response = client.DeleteAsync($"SubjectApi/{id}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<SubjectViewModel>>(data);

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
                baseResponse.Result = new SubjectViewModel();
                return baseResponse;
            }
        }
    }
}
