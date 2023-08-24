using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Model.Subjects;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface ISubjectApiRepository
    {
        BaseResponseModel<SubjectModel> GetAllSubjects();
        Task<BaseResponseModel<PaggedSubjectModel>> GetAll(SearchingPara para);
        Task<BaseResponseModel<SubjectModel>> Get(int id);
        Task<BaseResponseModel<SubjectModel>> Add(SubjectModel subject);
        Task<BaseResponseModel<SubjectModel>> Update(SubjectModel subject);
        Task<BaseResponseModel<SubjectModel>> Delete(int id);
    }
}
