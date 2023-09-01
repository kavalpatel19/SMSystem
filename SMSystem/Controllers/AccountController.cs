using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SMSystem.Models.Auth;
using SMSystem.Repository.Interfaces;
using SMSystem.Models;

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
                    // Add other claims as needed
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(90) // Set expiration time
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    var user = stdRepo.GetAllStudents().Results.Where(x => x.StudentId == response.Result.UserId).FirstOrDefault();

                    TempData["Username"] = user.FirstName;
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account"); // Redirect after logout
        }
    }
}
