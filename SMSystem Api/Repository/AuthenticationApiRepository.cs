using SMSystem_Api.Data;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Auth;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Repository.Interfaces;

namespace SMSystem_Api.Repository
{
    public class AuthenticationApiRepository : IAuthenticationApiRepository
    {
        private readonly ApplicationDbContext context;

        public AuthenticationApiRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public BaseResponseModel<ApplicationUser> Login(LoginModel model)
        {
            var response = new BaseResponseModel<ApplicationUser>();
            try
            {
                var user = context.User.Where( x => x.Username == model.Username ).FirstOrDefault();
                if(user != null)
                {
                    if(model.Username != user.Username)
                    {
                        response.ResponseCode = 500;
                        response.Message = "Invalid Username!";
                        return response;
                    }
                    if(model.Password != user.Password)
                    {
                        response.ResponseCode = 500;
                        response.Message = "Invalid Password!";
                        return response;
                    }

                    response.ResponseCode = 200;
                    response.Result = user;
                    return response;
                }
                response.ResponseCode = 500;
                response.Message = "User Does not Exist!";
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
    }
}
