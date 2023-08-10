using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Holiday;

namespace SMSystem.Repository.Interfaces
{
    public interface IHolidayRepository
    {
        List<HolidayViewModel> GetAllHolidays();
        Task<HolidayPaggedViewModel> GetHolidays(SearchingParaModel para);
        Task<bool> Add(HolidayViewModel holiday);
    }
}
