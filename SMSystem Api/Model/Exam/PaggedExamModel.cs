using SMSystem_Api.Model.Department;

namespace SMSystem_Api.Model.Exam
{
    public class PaggedExamModel
    {
        public IList<ExamModel> ExamModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
