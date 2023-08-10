using SMSystem.Helpers;
using SMSystem.Models.Students;

namespace SMSystem.Repository.Interfaces
{
    public interface IStudentRepository
    {
        List<StudentViewModel> GetAllStudents();
        Task<StudentPagedViewModel> GetStudents(SearchingParaModel para);
        Task<StudentViewModel> GetStudent(int id);
        Task<bool> Add(StudentViewModel student);
        Task<bool> Update(StudentViewModel student);
        Task<bool> Delete(int id);
    }
}
