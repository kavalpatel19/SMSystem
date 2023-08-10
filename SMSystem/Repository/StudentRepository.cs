using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.Differencing;
using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models.Students;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public StudentRepository(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        public List<StudentViewModel> GetAllStudents()
        {
            var student = new List<StudentViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = client.GetAsync($"StudentApi/Export").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    student = JsonConvert.DeserializeObject<List<StudentViewModel>>(data);
                }

                return student;
            }
        }

        public async Task<StudentPagedViewModel> GetStudents(SearchingParaModel para)
        {
            StudentPagedViewModel studentPagedViewModel = new StudentPagedViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);

                para.SId = string.IsNullOrEmpty(para.SId) ? string.Empty : para.SId;
                para.Name = string.IsNullOrEmpty(para.Name) ? string.Empty : para.Name;
                para.Phone = string.IsNullOrEmpty(para.Phone) ? string.Empty : para.Phone;

                var response = await client.GetAsync($"StudentApi?sid={para.SId}&name={para.Name}&phone={para.Phone}&pageIndex={para.PageIndex}");

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    studentPagedViewModel = JsonConvert.DeserializeObject<StudentPagedViewModel>(data);
                }

                return studentPagedViewModel;
            }
        }

        public async Task<StudentViewModel> GetStudent(int id)
        {
            StudentViewModel student = new StudentViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetSection("ApiUrl").Value);
                var response = await client.GetAsync($"StudentApi/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    student = JsonConvert.DeserializeObject<StudentViewModel>(data);
                }

                return student;
            }
        }

        public async Task<bool> Add(StudentViewModel student)
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> Update(StudentViewModel student)
        {
            if (student.ImagePath != null)
            {
                if(student.Path == null)
                {
                    student.Path = UploadImage(student);
                }
                else if (student.Path.Contains("/Default/"))
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
                var response = client.DeleteAsync($"StudentApi/{id}").Result;

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
