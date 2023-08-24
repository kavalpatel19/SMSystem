using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Holiday;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Students;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IStudentApiRepository
    {
        BaseResponseModel<StudentModel> GetAllStudents();
        Task<BaseResponseModel<PaggedStudentModel>> GetAll(SearchingPara para);
        Task<BaseResponseModel<StudentModel>> Get(int id);
        Task<BaseResponseModel<StudentModel>> Add(StudentModel student);
        Task<BaseResponseModel<StudentModel>> Update(StudentModel student);
        Task<BaseResponseModel<StudentModel>> Delete(int id);
    }
}
