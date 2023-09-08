using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Students;
using SMSystem.Models.Subject;
using SMSystem.Models.Teacher;

namespace SMSystem.Repository.Interfaces
{
    public interface ITeacherRepository
    {
        BaseResponseViewModel<TeacherViewModel> GetAllTeachers();
        Task<BaseResponseViewModel<TeacherPagedViewModel>> GetTeachers(SearchingParaModel para);
        Task<BaseResponseViewModel<TeacherViewModel>> GetTeacher(int id);
        Task<BaseResponseViewModel<TeacherRegisterViewModel>> Add(TeacherRegisterViewModel teacher);
        Task<BaseResponseViewModel<TeacherViewModel>> Update(TeacherViewModel teacher);
        Task<BaseResponseViewModel<TeacherViewModel>> Delete(int id);
    }
}
