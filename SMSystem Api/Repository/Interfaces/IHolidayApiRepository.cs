using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Fees;
using SMSystem_Api.Model.Holiday;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IHolidayApiRepository
    {
        BaseResponseModel<HolidayModel> GetAllHolidays();
        Task<BaseResponseModel<PaggedHolidayModel>> GetAll(SearchingPara para);
        Task<BaseResponseModel<HolidayModel>> Add(HolidayModel holiday);
    }
}
