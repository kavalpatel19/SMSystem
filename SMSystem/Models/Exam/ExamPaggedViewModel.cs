using SMSystem.Models.Fees;

namespace SMSystem.Models.Exam
{
    public class ExamPaggedViewModel
    {
        public IList<ExamViewModel> ExamModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
