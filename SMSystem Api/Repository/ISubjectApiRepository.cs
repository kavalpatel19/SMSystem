using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Subjects;

namespace SMSystem_Api.Repository
{
    public interface ISubjectApiRepository
    {
        List<SubjectModel> GetAllSubjects();
        Task<PaggedSubjectModel> GetAll(SearchingPara para);
        Task<SubjectModel> Get(int id);
        Task Add(SubjectModel subject);
        Task Update(SubjectModel subject);
        Task Delete(int id);
    }
}
