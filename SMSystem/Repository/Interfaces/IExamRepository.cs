using SMSystem.Helpers;
using SMSystem.Models.Exam;
using SMSystem.Models.Fees;

namespace SMSystem.Repository.Interfaces
{
    public interface IExamRepository
    {
        List<ExamViewModel> GetAllExams();
        Task<ExamPaggedViewModel> GetExams(SearchingParaModel para);
        Task<ExamViewModel> GetExam(int id);
        Task<bool> Add(ExamViewModel exam);
        Task<bool> Update(ExamViewModel exam);
        Task<bool> Delete(int id);
    }
}
