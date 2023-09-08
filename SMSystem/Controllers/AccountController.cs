using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SMSystem.Models.Auth;
using SMSystem.Repository.Interfaces;
using SMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using SMSystem.Models.Students;
using Azure;

namespace SMSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationRepository authRepo;
        private readonly IStudentRepository stdRepo;
        private readonly ITeacherRepository teachRepo; 
         
        public AccountController(IAuthenticationRepository AuthRepo, IStudentRepository StdRepo,ITeacherRepository TeachRepo)
        {
            authRepo = AuthRepo;
            stdRepo = StdRepo;
            teachRepo = TeachRepo;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if(userId != null)
            {
                if (role.Equals("Teacher"))
                {
                    var teacher = teachRepo.GetAllTeachers().Results.Find(x => x.TeacherId == userId);
                    var res = authRepo.GetUsers().Results.Find (x => x.UserId == userId);
                    var address = $"{teacher.AddressLine1},\n{teacher.City},\n{teacher.Country},\n{teacher.PostalCode}.";
                    var user = new UserModel()
                    {
                        Name =  teacher.Name,
                        DOB = teacher.DateOfBirth,
                        Path = teacher.Path,
                        Email = res.Email,
                        Phone = teacher.PhoneNo,
                        Role = "Teacher",
                        Address = address
                    };
                    return View(user);
                }
                
                if (role.Equals("Student"))
                {
                    var student = stdRepo.GetAllStudents().Results.Find(x => x.StudentId == userId);
                    var res = authRepo.GetUsers().Results.Find (x => x.UserId == userId);
                    var name = student.FirstName +" "+ student.LastName;
                    var user = new UserModel()
                    {
                        Name =  name,
                        DOB = student.DateOfBirth,
                        Path = student.Path,
                        Email = res.Email,
                        Phone = student.Phone,
                        Role = "Student",
                        Address = student.Address
                    };
                    return View(user);
                }
            }
            TempData["Message"] = "Something's Wrong!";
            TempData["ResCode"] = 500;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var response = new BaseResponseViewModel<ApplicationUser>();
            try
            {
                response = authRepo.Login(model);
                if (response.ResponseCode == 200)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, response.Result.UserId),
                        new Claim(ClaimTypes.Role, response.Result.Role),
                        new Claim(ClaimTypes.GivenName, response.Result.Name), 
                        new Claim(ClaimTypes.Uri,"/images/Default/Admin.jfif"),

                    };

                    if (response.Result.Role == "Student")
                    {
                        var student = stdRepo.GetAllStudents().Results.Find(x => x.StudentId == response.Result.UserId);

                        claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, response.Result.UserId),
                            new Claim(ClaimTypes.Role, response.Result.Role),
                            new Claim(ClaimTypes.GivenName, response.Result.Name),
                            new Claim(ClaimTypes.Uri, student.Path),
                            // Add other claims as needed
                        };
                    }
                    if (response.Result.Role == "Teacher")
                    {
                        var teacher = teachRepo.GetAllTeachers().Results.Find(x => x.TeacherId == response.Result.UserId);

                        claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, response.Result.UserId),
                            new Claim(ClaimTypes.Role, response.Result.Role),
                            new Claim(ClaimTypes.GivenName, response.Result.Name),
                            new Claim(ClaimTypes.Uri, teacher.Path),
                            // Add other claims as needed
                        };
                    }
                    

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(90) // Set expiration time
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToAction("Index", "Home"); // Redirect after successful login
                }

                TempData["Message"] = response.Message;
                TempData["ResCode"] = response.ResponseCode;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Please contect ADMIN!";
                TempData["ResCode"] = 500;
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account"); // Redirect after logout
        }

        public ActionResult ChangePassword()
        {
            var model = new PasswordModel();
            return PartialView("_ChangePassword",model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PasswordModel model)
        {
            model.UserId = User.FindFirst(ClaimTypes.Name)?.Value;
            var response = authRepo.ChangePassword(model);
            if(response.ResponseCode == 200)
            {
                TempData["ResCode"] = 200;
                TempData["Message"] = "Password Changed Successfully.";
                return RedirectToAction("Profile");
            }
            TempData["ResCode"] = 500;
            TempData["Message"] = response.Message;
            return RedirectToAction("Profile");
        }

        [Authorize]
        public IActionResult EmailExist(string email, int id)
        {
            var users = authRepo.GetUsers().Results.Where(x => x.Email == email).FirstOrDefault();
            if (users != null)
            {
                if (id > 0 && id == users.Id)
                {
                    return Json(true);
                }

                return Json("Email Already Exist!");
            }
            return Json(true);
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            TempData["Message"] = "Access Denied";
            TempData["ResCode"] = 500;
            return RedirectToAction("Index", "Home");
        }
    }
}
