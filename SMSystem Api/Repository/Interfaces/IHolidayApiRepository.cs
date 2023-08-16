using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Holiday;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IHolidayApiRepository
    {
        List<HolidayModel> GetAllHolidays();
        Task<PaggedHolidayModel> GetAll(SearchingPara para);
        Task Add(HolidayModel holiday);
    }
}
