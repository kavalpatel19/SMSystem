using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SMSystem.Models.Auth;
using SMSystem.Repository.Interfaces;

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
            var response = authRepo.Login(model);
            // Perform user authentication here (e.g., check credentials against a database)
            // If authentication succeeds, sign in the user using SignInAsync method
            // For example:
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
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20) // Set expiration time
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                var user = stdRepo.GetAllStudents().Results.Where(x => x.Id == response.Result.UserId).FirstOrDefault();

                TempData["Username"] = user.FirstName;
                return RedirectToAction("Index", "Home"); // Redirect after successful login
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Redirect after logout
        }
    }
}
