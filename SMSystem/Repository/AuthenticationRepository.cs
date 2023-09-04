using Newtonsoft.Json;
using SMSystem.Models;
using SMSystem.Models.Auth;
using SMSystem.Models.Department;
using SMSystem.Repository.Interfaces;
using System.Configuration;

namespace SMSystem.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IConfiguration configuration;

        public AuthenticationRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public BaseResponseViewModel<ApplicationUser> GetUsers()
        {
            var baseResponse = new BaseResponseViewModel<ApplicationUser>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.GetAsync("AccountApi").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<ApplicationUser>>(data);
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
                baseResponse.Result = new ApplicationUser();
                return baseResponse;
            }
        }

        public BaseResponseViewModel<ApplicationUser> Login(LoginViewModel model)
        {
            var baseResponse = new BaseResponseViewModel<ApplicationUser>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PostAsJsonAsync<LoginViewModel>("AccountApi/Login", model).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<ApplicationUser>>(data);
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
                baseResponse.Result = new ApplicationUser();
                return baseResponse;
            }
        }
    }
}
