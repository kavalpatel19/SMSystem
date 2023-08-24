using SMSystem_Api.Model.Holiday;

namespace SMSystem_Api.Model.Students
{
    public class PaggedStudentModel
    {
        public PaggedStudentModel()
        {
            StudentModel = new List<StudentModel>();
            PaggedModel = new PaggedModel();
        }
        public IList<StudentModel> StudentModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
