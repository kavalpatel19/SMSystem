using SMSystem.Models.Department;

namespace SMSystem.Models.Holiday
{
    public class HolidayPaggedViewModel
    {
        public IList<HolidayViewModel> HolidayModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
