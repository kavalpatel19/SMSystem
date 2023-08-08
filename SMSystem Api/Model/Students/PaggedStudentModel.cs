namespace SMSystem_Api.Model.Students
{
    public class PaggedStudentModel
    {
        public IList<StudentModel> StudentModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
