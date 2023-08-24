using SMSystem.Models.Department;
using SMSystem.Models.Students;

namespace SMSystem.Models.Teacher
{
    public class TeacherPagedViewModel
    {
        public TeacherPagedViewModel()
        {
            TeacherModel = new List<TeacherViewModel>();
            PaggedModel = new PaggedViewModel();
        }
        public List<TeacherViewModel> TeacherModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
