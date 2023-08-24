﻿using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Exam;
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

        public BaseResponseViewModel<HolidayViewModel> GetAllHolidays()
        {
            var baseResponse = new BaseResponseViewModel<HolidayViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.GetAsync($"HolidayApi/Export").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<HolidayViewModel>>(data);
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
                baseResponse.Results = new List<HolidayViewModel>();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<HolidayPaggedViewModel>> GetHolidays(SearchingParaModel para)
        {
            var baseResponse = new BaseResponseViewModel<HolidayPaggedViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    var response = await client.GetAsync($"HolidayApi?pageIndex={para.PageIndex}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<HolidayPaggedViewModel>>(data);
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
                baseResponse.Result = new HolidayPaggedViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<HolidayViewModel>> Add(HolidayViewModel holiday)
        {
            var baseResponse = new BaseResponseViewModel<HolidayViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PostAsJsonAsync<HolidayViewModel>("HolidayApi/", holiday).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<HolidayViewModel>>(data);
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
                baseResponse.Result = new HolidayViewModel();
                return baseResponse;
            }
        }
    }
}
