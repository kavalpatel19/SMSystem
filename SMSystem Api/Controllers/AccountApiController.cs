﻿
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Auth;
using SMSystem_Api.Repository.Interfaces;

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly IAuthenticationApiRepository authRepo;

        public AccountApiController(IAuthenticationApiRepository AuthRepo)
        {
            authRepo = AuthRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers() 
        {
            var baseResponse = new BaseResponseModel<ApplicationUser>();
            try
            {
                baseResponse =  await authRepo.GetUsers();
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new ApplicationUser();
                return Ok(baseResponse);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginModel model)
        {
            var baseResponse = new BaseResponseModel<ApplicationUser>();
            try
            {
                baseResponse = authRepo.Login(model);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new ApplicationUser();
                return Ok(baseResponse);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(ApplicationUser user)
        {
            var baseResponse = new BaseResponseModel<ApplicationUser>();
            try
            {
                baseResponse = await authRepo.Register(user);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new ApplicationUser();
                return Ok(baseResponse);
            }
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(PasswordModel model)
        {
            var baseResponse = new BaseResponseModel<ApplicationUser>();
            try
            {
                baseResponse = authRepo.ChangePassword(model);
                return Ok(baseResponse);
            }
            catch(Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new ApplicationUser();
                return Ok(baseResponse);
            }
        }

    }
}
