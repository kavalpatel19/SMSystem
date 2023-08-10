using Newtonsoft.Json;
using SMSystem.Helpers;
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

        public List<TeacherViewModel> GetAllTeachers()
        {
            var teachers = new List<TeacherViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.GetAsync($"TeacherApi/Export").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(data);
                }

                return teachers;
            }
        }

        public async Task<TeacherPagedViewModel> GetTeachers(SearchingParaModel para)
        {
            TeacherPagedViewModel teacherPagedViewModel = new TeacherPagedViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                para.SId = string.IsNullOrEmpty(para.SId) ? string.Empty : para.SId;
                para.Name = string.IsNullOrEmpty(para.Name) ? string.Empty : para.Name;
                para.Phone = string.IsNullOrEmpty(para.Phone) ? string.Empty : para.Phone;

                var response = await client.GetAsync($"TeacherApi?sid={para.SId}&name={para.Name}&phone={para.Phone}&pageIndex={para.PageIndex}");

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    teacherPagedViewModel = JsonConvert.DeserializeObject<TeacherPagedViewModel>(data);
                }

                return teacherPagedViewModel;
            }
        }

        public async Task<TeacherViewModel> GetTeacher(int id)
        {
            TeacherViewModel teacher = new TeacherViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = await client.GetAsync($"TeacherApi/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    teacher = JsonConvert.DeserializeObject<TeacherViewModel>(data);
                }

                return teacher;
            }
        }

        public async Task<bool> Add(TeacherViewModel teacher)
        {
            string uniqueFileName = string.Empty;

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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                var response = client.PostAsJsonAsync<TeacherViewModel>("TeacherApi", teacher).Result;

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
        public async Task<bool> Update(TeacherViewModel teacher)
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
                var response = client.DeleteAsync($"TeacherApi/{id}").Result;

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
