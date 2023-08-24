using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Exam;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IExamApiRepository
    {
        BaseResponseModel<ExamModel> GetAllExams();
        Task<BaseResponseModel<PaggedExamModel>> GetAll(SearchingPara para);
        Task<BaseResponseModel<ExamModel>> Get(int id);
        Task<BaseResponseModel<ExamModel>> Add(ExamModel exam);
        Task<BaseResponseModel<ExamModel>> Update(ExamModel exam);
        Task<BaseResponseModel<ExamModel>> Delete(int id);
    }
}
