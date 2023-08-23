using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Fees;
using SMSystem.Models.Holiday;

namespace SMSystem.Repository.Interfaces
{
    public interface IHolidayRepository
    {
        BaseResponseViewModel<HolidayViewModel> GetAllHolidays();
        Task<BaseResponseViewModel<HolidayPaggedViewModel>> GetHolidays(SearchingParaModel para);
        Task<BaseResponseViewModel<HolidayViewModel>> Add(HolidayViewModel holiday);
    }
}
