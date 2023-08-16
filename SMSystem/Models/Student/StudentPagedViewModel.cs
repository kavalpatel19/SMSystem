using SMSystem.Models.Department;

namespace SMSystem.Models.Students
{
    public class StudentPagedViewModel
    {
        public StudentPagedViewModel()
        {
            StudentModel = new List<StudentViewModel>();
            PaggedModel = new PaggedViewModel();
        }
        public List<StudentViewModel> StudentModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}