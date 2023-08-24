using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Students;
using SMSystem.Models.Subject;

namespace SMSystem.Repository.Interfaces
{
    public interface ISubjectRepository
    {
        BaseResponseViewModel<SubjectViewModel> GetAllSubjects();
        Task<BaseResponseViewModel<SubjectPaggedViewModel>> GetSubjects(SearchingParaModel para);
        Task<BaseResponseViewModel<SubjectViewModel>> GetSubject(int id);
        Task<BaseResponseViewModel<SubjectViewModel>> Add(SubjectViewModel subject);
        Task<BaseResponseViewModel<SubjectViewModel>> Update(SubjectViewModel subject);
        Task<BaseResponseViewModel<SubjectViewModel>> Delete(int id);
    }
}
