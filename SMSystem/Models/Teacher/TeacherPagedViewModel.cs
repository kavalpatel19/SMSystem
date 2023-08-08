using SMSystem.Models.Students;

namespace SMSystem.Models.Teacher
{
    public class TeacherPagedViewModel
    {
        public List<TeacherViewModel> TeacherModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
