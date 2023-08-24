using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Model.Subjects;
using SMSystem_Api.Model.Teachers;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface ITeacherApiRepository
    {
        BaseResponseModel<TeacherModel> GetAllTeachers();
        Task<BaseResponseModel<PaggedTeacherModel>> GetAll(SearchingPara para);
        Task<BaseResponseModel<TeacherModel>> Get(int id);
        Task<BaseResponseModel<TeacherModel>> Add(TeacherModel teacher);
        Task<BaseResponseModel<TeacherModel>> Update(TeacherModel teacher);
        Task<BaseResponseModel<TeacherModel>> Delete(int id);
    }
}
