using SMSystem_Api.Model.Department;

namespace SMSystem_Api.Model.Exam
{
    public class PaggedExamModel
    {
        public PaggedExamModel()
        {
            ExamModel = new List<ExamModel>();
            PaggedModel = new PaggedModel();
        }
        public IList<ExamModel> ExamModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
