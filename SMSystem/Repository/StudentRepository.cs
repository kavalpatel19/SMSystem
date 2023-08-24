using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.Differencing;
using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Exam;
using SMSystem.Models.Students;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public StudentRepository(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        public BaseResponseViewModel<StudentViewModel> GetAllStudents()
        {
            var baseResponse = new BaseResponseViewModel<StudentViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.GetAsync($"StudentApi/Export").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<StudentViewModel>>(data);
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
                baseResponse.Results = new List<StudentViewModel>();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<StudentPagedViewModel>> GetStudents(SearchingParaModel para)
        {
            var baseResponse = new BaseResponseViewModel<StudentPagedViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    para.SId = string.IsNullOrEmpty(para.SId) ? string.Empty : para.SId;
                    para.Name = string.IsNullOrEmpty(para.Name) ? string.Empty : para.Name;
                    para.Phone = string.IsNullOrEmpty(para.Phone) ? string.Empty : para.Phone;

                    var response = await client.GetAsync($"StudentApi?sid={para.SId}&name={para.Name}&phone={para.Phone}&pageIndex={para.PageIndex}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<StudentPagedViewModel>>(data);
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
                baseResponse.Result = new StudentPagedViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<StudentViewModel>> GetStudent(int id)
        {
            var baseResponse = new BaseResponseViewModel<StudentViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    var response = await client.GetAsync($"StudentApi/{id}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<StudentViewModel>>(data);
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
                baseResponse.Result = new StudentViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<StudentViewModel>> Add(StudentViewModel student)
        {
            var baseResponse = new BaseResponseViewModel<StudentViewModel>();
            try
            {
                string uniqueFileName = string.Empty;

                if (student.ImagePath == null)
                {
                    if (student.Gender.Equals("Male"))
                    {
                        uniqueFileName = "/images/Default/MaleAvatar.png";
                    }
                    else
                    {
                        uniqueFileName = "/images/Default/FemaleAvatqar.jfif";
                    }
                }
                else
                {
                    uniqueFileName = UploadImage(student);
                }

                student.Path = uniqueFileName;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PostAsJsonAsync<StudentViewModel>("StudentApi", student).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<StudentViewModel>>(data);
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
                baseResponse.Result = new StudentViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<StudentViewModel>> Update(StudentViewModel student)
        {
            var baseResponse = new BaseResponseViewModel<StudentViewModel>();
            try
            {
                if (student.ImagePath != null)
                {
                    if (student.Path == null || student.Path.Contains("/Default/"))
                    {
                        student.Path = UploadImage(student);
                    }
                    else
                    {
                        string FilePath = $"{environment.WebRootPath}{student.Path}";
                        if (System.IO.File.Exists(FilePath))
                        {
                            System.IO.File.Delete(FilePath);
                        }
                        student.Path = UploadImage(student);
                    }
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PutAsJsonAsync<StudentViewModel>("StudentApi", student).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<StudentViewModel>>(data);

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
                baseResponse.Result = new StudentViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<StudentViewModel>> Delete(int id)
        {
            var baseResponse = new BaseResponseViewModel<StudentViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    // To send Delete data request
                    var response = client.DeleteAsync($"StudentApi/{id}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<StudentViewModel>>(data);

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
                baseResponse.Result = new StudentViewModel();
                return baseResponse;
            }
        }

        private string UploadImage(StudentViewModel model)
        {
            string uniqueFileName = string.Empty;
            if (model.ImagePath != null)
            {
                uniqueFileName = $"/images/Students/{ Guid.NewGuid().ToString()}_{model.ImagePath.FileName}";
                string FilePath = $"{environment.WebRootPath}{uniqueFileName}";
                using (var fileStream = new FileStream(FilePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
