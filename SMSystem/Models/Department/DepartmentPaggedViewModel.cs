namespace SMSystem.Models.Department
{
    public class DepartmentPaggedViewModel
    {
        public DepartmentPaggedViewModel()
        {
            DepartmentModel = new List<DepartmentViewModel>();
            PaggedModel = new PaggedViewModel();
        }
        public IList<DepartmentViewModel> DepartmentModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
