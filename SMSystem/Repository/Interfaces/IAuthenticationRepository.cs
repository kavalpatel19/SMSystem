﻿using SMSystem.Models;
using SMSystem.Models.Auth;

namespace SMSystem.Repository.Interfaces
{
    public interface IAuthenticationRepository
    {
        public BaseResponseViewModel<ApplicationUser> Login(LoginViewModel model);
        public BaseResponseViewModel<ApplicationUser> GetUsers();
        public BaseResponseViewModel<ApplicationUser> ChangePassword(PasswordModel model);

    }
}
