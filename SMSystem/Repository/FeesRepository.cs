using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Exam;
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

        public BaseResponseViewModel<FeesViewModel> GetAllFees()
        {
            var baseResponse = new BaseResponseViewModel<FeesViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.GetAsync($"FeesApi/Export").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<FeesViewModel>>(data);
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
                baseResponse.Results = new List<FeesViewModel>();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<FeesPaggedViewModel>> GetFees(SearchingParaModel para)
        {
            var baseResponse = new BaseResponseViewModel<FeesPaggedViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    var response = await client.GetAsync($"FeesApi?pageIndex={para.PageIndex}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<FeesPaggedViewModel>>(data);
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
                baseResponse.Result = new FeesPaggedViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<FeesViewModel>> GetFee(int id)
        {
            var baseResponse = new BaseResponseViewModel<FeesViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    var response = await client.GetAsync($"FeesApi/{id}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<FeesViewModel>>(data);
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
                baseResponse.Result = new FeesViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<FeesViewModel>> Add(FeesViewModel fee)
        {
            var baseResponse = new BaseResponseViewModel<FeesViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PostAsJsonAsync<FeesViewModel>("FeesApi/", fee).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<FeesViewModel>>(data);
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
                baseResponse.Result = new FeesViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<FeesViewModel>> Update(FeesViewModel fee)
        {
            var baseResponse = new BaseResponseViewModel<FeesViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PutAsJsonAsync<FeesViewModel>("FeesApi/", fee).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<FeesViewModel>>(data);

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
                baseResponse.Result = new FeesViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<FeesViewModel>> Delete(int id)
        {
            var baseResponse = new BaseResponseViewModel<FeesViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    // To send Delete data request
                    var response = client.DeleteAsync($"FeesApi/{id}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<FeesViewModel>>(data);

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
                baseResponse.Result = new FeesViewModel();
                return baseResponse;
            }
        }
    }
}
