using SMSystem_Api.Model;
using SMSystem_Api.Model.Auth;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IAuthenticationApiRepository
    {
        public BaseResponseModel<ApplicationUser> Login(LoginModel model);
        public Task<BaseResponseModel<ApplicationUser>> Register(ApplicationUser model);

    }
}
