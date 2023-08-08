namespace SMSystem.Models.Department
{
    public class DepartmentPaggedViewModel
    {
        public IList<DepartmentViewModel> DepartmentModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
