using DocumentFormat.OpenXml.Math;
using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models;
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

        public BaseResponseViewModel<DepartmentViewModel> GetAllDepartments()
        {
            var baseResponse = new BaseResponseViewModel<DepartmentViewModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.GetAsync($"DepartmentApi/Export").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<DepartmentViewModel>>(data);
                    }

                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Results = new List<DepartmentViewModel>();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<DepartmentPaggedViewModel>> GetDepartmnets(SearchingParaModel para)
        {
            var baseResponse = new BaseResponseViewModel<DepartmentPaggedViewModel>();
            try
            {
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
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<DepartmentPaggedViewModel>>(data);
                    }

                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new DepartmentPaggedViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<DepartmentViewModel>> GetDepartment(int id)
        {
            var baseResponse = new BaseResponseViewModel<DepartmentViewModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    var response = await client.GetAsync($"DepartmentApi/{id}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<DepartmentViewModel>>(data);
                    }
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new DepartmentViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<DepartmentViewModel>> Add(DepartmentViewModel department)
        {
            var baseResponse = new BaseResponseViewModel<DepartmentViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = await client.PostAsJsonAsync<DepartmentViewModel>("DepartmentApi/", department);
                    
                    var data = response.Content.ReadAsStringAsync().Result;
                    baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<DepartmentViewModel>>(data);

                    return baseResponse;
                }
            }
            catch(Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new DepartmentViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<DepartmentViewModel>> Update(DepartmentViewModel department)
        {
            var baseResponse = new BaseResponseViewModel<DepartmentViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PutAsJsonAsync<DepartmentViewModel>("DepartmentApi/", department).Result;

                    var data = response.Content.ReadAsStringAsync().Result;
                    baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<DepartmentViewModel>>(data);

                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new DepartmentViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<DepartmentViewModel>> Delete(int id)
        {
            var baseResponse = new BaseResponseViewModel<DepartmentViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    // To send Delete data request
                    var response = client.DeleteAsync($"DepartmentApi/{id}").Result;

                    var data = response.Content.ReadAsStringAsync().Result;
                    baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<DepartmentViewModel>>(data);

                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new DepartmentViewModel();
                return baseResponse;
            }
        }
    }
}
