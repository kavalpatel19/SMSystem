using SMSystem_Api.Model.Students;

namespace SMSystem_Api.Model.Teachers
{
    public class PaggedTeacherModel
    {
        public IList<TeacherModel> TeacherModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
