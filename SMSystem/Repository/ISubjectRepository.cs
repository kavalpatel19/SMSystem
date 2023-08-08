using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Subject;

namespace SMSystem.Repository
{
    public interface ISubjectRepository
    {
        List<SubjectViewModel> GetAllSubjects();
        Task<SubjectPaggedViewModel> GetSubjects(SearchingParaModel para);
        Task<SubjectViewModel> GetSubject(int id);
        Task<bool> Add(SubjectViewModel subject);
        Task<bool> Update(SubjectViewModel subject);
        Task<bool> Delete(int id);
    }
}
