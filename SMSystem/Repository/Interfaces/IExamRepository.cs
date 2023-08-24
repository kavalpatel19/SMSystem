using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models;
using SMSystem.Models.Exam;
using SMSystem.Models.Fees;

namespace SMSystem.Repository.Interfaces
{
    public interface IExamRepository
    {
        BaseResponseViewModel<ExamViewModel> GetAllExams();
        Task<BaseResponseViewModel<ExamPaggedViewModel>> GetExams(SearchingParaModel para);
        Task<BaseResponseViewModel<ExamViewModel>> GetExam(int id);
        Task<BaseResponseViewModel<ExamViewModel>> Add(ExamViewModel exam);
        Task<BaseResponseViewModel<ExamViewModel>> Update(ExamViewModel exam);
        Task<BaseResponseViewModel<ExamViewModel>> Delete(int id);
    }
}
