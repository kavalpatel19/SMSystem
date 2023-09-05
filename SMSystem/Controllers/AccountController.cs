using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SMSystem.Models.Auth;
using SMSystem.Repository.Interfaces;
using SMSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace SMSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationRepository authRepo;
        private readonly IStudentRepository stdRepo;

        public AccountController(IAuthenticationRepository AuthRepo, IStudentRepository StdRepo)
        {
            authRepo = AuthRepo;
            stdRepo = StdRepo;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if(userId != null)
            {
                if (role.Equals("student"))
                {
                    var user = stdRepo.GetAllStudents().Results.Find(x => x.StudentId == userId);

                }
                
            }
            return View();
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
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, response.Result.Role),
                    new Claim(ClaimTypes.GivenName, response.Result.Name),
                    // Add other claims as needed
                };

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
                TempData["Message"] = ex.Message;
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
