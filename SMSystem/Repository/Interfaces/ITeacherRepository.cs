using SMSystem.Helpers;
using SMSystem.Models.Students;
using SMSystem.Models.Teacher;

namespace SMSystem.Repository.Interfaces
{
    public interface ITeacherRepository
    {
        List<TeacherViewModel> GetAllTeachers();
        Task<TeacherPagedViewModel> GetTeachers(SearchingParaModel para);
        Task<TeacherViewModel> GetTeacher(int id);
        Task<bool> Add(TeacherViewModel teacher);
        Task<bool> Update(TeacherViewModel teacher);
        Task<bool> Delete(int id);
    }
}
