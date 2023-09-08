﻿using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Auth;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Repository.Interfaces;
using System.Security.Claims;

namespace SMSystem_Api.Repository
{
    public class AuthenticationApiRepository : IAuthenticationApiRepository
    {
        private readonly ApplicationDbContext context;

        public AuthenticationApiRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<BaseResponseModel<ApplicationUser>> GetUsers()
        {
            var response = new BaseResponseModel<ApplicationUser>();
            try
            {
                response.Results = context.User.ToList();
                response.ResponseCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<BaseResponseModel<ApplicationUser>> Register(ApplicationUser model)
        {
            var response = new BaseResponseModel<ApplicationUser>();
            try
            {
                await context.User.AddAsync(model).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                response.ResponseCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                return response;
            }
        }

        public BaseResponseModel<ApplicationUser> Login(LoginModel model)
        {
            var response = new BaseResponseModel<ApplicationUser>();
            try
            {
                var user = context.User.Where( x => x.Username == model.Username || x.Email == model.Username).FirstOrDefault();
                if(user != null)
                {
                    if(model.Password != user.Password)
                    {
                        response.ResponseCode = 500;
                        response.Message = "Invalid Password for this username!";
                        return response;
                    }

                    response.ResponseCode = 200;
                    response.Result = user;
                    return response;
                }
                response.ResponseCode = 500;
                response.Message = "Username does not Exist!";
                return response;
            }
            catch(Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new ApplicationUser();
                return response;
            }
        }

        public BaseResponseModel<ApplicationUser> ChangePassword(PasswordModel model)
        {
            var response = new BaseResponseModel<ApplicationUser>();
            try
            {
                var user = context.User.Where(x => x.UserId == model.UserId).FirstOrDefault();
                if(user != null)
                {
                    if(user.Password == model.OldPassword)
                    {
                        user.Password = model.NewPassword;
                        context.Attach(user).State = EntityState.Modified;
                        context.SaveChanges();
                        response.ResponseCode = 200;
                        return response;
                    }
                    response.ResponseCode = 500;
                    response.Message = "Old password is wrong!";
                    return response;
                }
                response.ResponseCode = 500;
                response.Message = "Something Went wrong!";
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
