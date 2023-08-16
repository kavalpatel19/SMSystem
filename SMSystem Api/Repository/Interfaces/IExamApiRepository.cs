using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Exam;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IExamApiRepository
    {
        List<ExamModel> GetAllExams();
        Task<PaggedExamModel> GetAll(SearchingPara para);
        Task<ExamModel> Get(int id);
        Task Add(ExamModel exam);
        Task Update(ExamModel exam);
        Task Delete(int id);
    }
}
