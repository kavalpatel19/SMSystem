using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Model.Teachers;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface ITeacherApiRepository
    {
        List<TeacherModel> GetAllTeachers();
        Task<PaggedTeacherModel> GetAll(SearchingPara para);
        Task<TeacherModel> Get(int id);
        Task Add(TeacherModel teacher);
        Task Update(TeacherModel teacher);
        Task Delete(int id);
    }
}
