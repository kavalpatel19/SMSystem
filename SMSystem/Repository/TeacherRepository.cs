using Azure;
using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Auth;
using SMSystem.Models.Exam;
using SMSystem.Models.Students;
using SMSystem.Models.Teacher;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public TeacherRepository(IConfiguration configuration,IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        public BaseResponseViewModel<TeacherViewModel> GetAllTeachers()
        {
            var baseResponse = new BaseResponseViewModel<TeacherViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.GetAsync($"TeacherApi/Export").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<TeacherViewModel>>(data);
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
                baseResponse.Results = new List<TeacherViewModel>();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<TeacherPagedViewModel>> GetTeachers(SearchingParaModel para)
        {
            var baseResponse = new BaseResponseViewModel<TeacherPagedViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    para.SId = string.IsNullOrEmpty(para.SId) ? string.Empty : para.SId;
                    para.Name = string.IsNullOrEmpty(para.Name) ? string.Empty : para.Name;
                    para.Phone = string.IsNullOrEmpty(para.Phone) ? string.Empty : para.Phone;

                    var response = await client.GetAsync($"TeacherApi?sid={para.SId}&name={para.Name}&phone={para.Phone}&pageIndex={para.PageIndex}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<TeacherPagedViewModel>>(data);
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
                baseResponse.Result = new TeacherPagedViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<TeacherViewModel>> GetTeacher(int id)
        {
            var baseResponse = new BaseResponseViewModel<TeacherViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                    var response = await client.GetAsync($"TeacherApi/{id}").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<TeacherViewModel>>(data);
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
                baseResponse.Result = new TeacherViewModel();
                return baseResponse;
            }
        }

        public async Task<BaseResponseViewModel<TeacherRegisterViewModel>> Add(TeacherRegisterViewModel register)
        {
            var baseResponse = new BaseResponseViewModel<TeacherRegisterViewModel>();
            try
            {
                string uniqueFileName = string.Empty;
                var teacher = register.TeacherModel;
                if (teacher.ImagePath == null)
                {
                    if (teacher.Gender.Equals("Male"))
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
                    uniqueFileName = UploadImage(teacher);
                }

                teacher.Path = uniqueFileName;
                register.UserModel.Role = "Teacher";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var teacherResponse = client.PostAsJsonAsync<TeacherViewModel>("TeacherApi", register.TeacherModel).Result;
                    var userResponse = client.PostAsJsonAsync<ApplicationUser>("AccountApi/Register", register.UserModel).Result;


                    if (teacherResponse.IsSuccessStatusCode && userResponse.IsSuccessStatusCode)
                    {
                        var tdata = teacherResponse.Content.ReadAsStringAsync().Result;
                        var tr = JsonConvert.DeserializeObject<BaseResponseViewModel<TeacherViewModel>>(tdata);                  
                        var udata = userResponse.Content.ReadAsStringAsync().Result;
                        var ur = JsonConvert.DeserializeObject<BaseResponseViewModel<ApplicationUser>>(udata);
                        if(tr.ResponseCode == 200 && ur.ResponseCode == 200)
                        {
                            baseResponse.ResponseCode = 200;
                            return baseResponse;
                        }
                        baseResponse.ResponseCode = 500;
                        baseResponse.Message = "Internal serrver error!";
                        return baseResponse;

                    }

                    baseResponse.ResponseCode = 400;
                    baseResponse.Message = "Bad Request";
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                baseResponse.ResponseCode = 500;
                baseResponse.Message = ex.Message;
                baseResponse.Result = new TeacherRegisterViewModel();
                return baseResponse;
            }
        }
        public async Task<BaseResponseViewModel<TeacherViewModel>> Update(TeacherViewModel teacher)
        {
            var baseResponse = new BaseResponseViewModel<TeacherViewModel>();
            try
            {
                if (teacher.ImagePath != null)
                {
                    if (teacher.Path == null)
                    {
                        teacher.Path = UploadImage(teacher);
                    }
                    else if (teacher.Path.Contains("/Default/"))
                    {
                        teacher.Path = UploadImage(teacher);
                    }
                    else
                    {
                        string FilePath = $"{environment.WebRootPath}{teacher.Path}";
                        if (System.IO.File.Exists(FilePath))
                        {
                            System.IO.File.Delete(FilePath);
                        }
                        teacher.Path = UploadImage(teacher);
                    }
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    var response = client.PutAsJsonAsync<TeacherViewModel>("TeacherApi", teacher).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<TeacherViewModel>>(data);

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
                baseResponse.Result = new TeacherViewModel();
                return baseResponse;
            }
        }


        public async Task<BaseResponseViewModel<TeacherViewModel>> Delete(int id)
        {
            var baseResponse = new BaseResponseViewModel<TeacherViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                    // To send Delete data request
                    var response = client.DeleteAsync($"TeacherApi/{id}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        baseResponse = JsonConvert.DeserializeObject<BaseResponseViewModel<TeacherViewModel>>(data);

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
                baseResponse.Result = new TeacherViewModel();
                return baseResponse;
            }
        }

        private string UploadImage(TeacherViewModel model)
        {
            string uniqueFileName = string.Empty;
            if (model.ImagePath != null)
            {
                uniqueFileName = $"/images/Teachers/{Guid.NewGuid().ToString()}_{model.ImagePath.FileName}";
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
