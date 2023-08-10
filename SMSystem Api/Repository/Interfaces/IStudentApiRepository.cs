using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Students;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IStudentApiRepository
    {
        List<StudentModel> GetAllStudents();
        Task<PaggedStudentModel> GetAll(SearchingPara para);
        Task<StudentModel> Get(int id);
        Task Add(StudentModel student);
        Task Update(StudentModel student);
        Task Delete(int id);
    }
}
