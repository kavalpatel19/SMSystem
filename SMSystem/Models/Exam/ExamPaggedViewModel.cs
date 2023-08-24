using SMSystem.Models.Fees;

namespace SMSystem.Models.Exam
{
    public class ExamPaggedViewModel
    {
        public ExamPaggedViewModel()
        {
            ExamModel = new List<ExamViewModel>();
            PaggedModel = new PaggedViewModel();
        }
        public IList<ExamViewModel> ExamModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
