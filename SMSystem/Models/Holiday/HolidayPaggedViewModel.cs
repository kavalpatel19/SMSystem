using SMSystem.Models.Department;

namespace SMSystem.Models.Holiday
{
    public class HolidayPaggedViewModel
    {
        public HolidayPaggedViewModel()
        {
            HolidayModel = new List<HolidayViewModel>();
            PaggedModel = new PaggedViewModel();
        }
        public IList<HolidayViewModel> HolidayModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
