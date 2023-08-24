using SMSystem_Api.Model.Students;
using SMSystem_Api.Model.Subjects;

namespace SMSystem_Api.Model.Teachers
{
    public class PaggedTeacherModel
    {
        public PaggedTeacherModel()
        {
            TeacherModel = new List<TeacherModel>();
            PaggedModel = new PaggedModel();
        }
        public IList<TeacherModel> TeacherModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
