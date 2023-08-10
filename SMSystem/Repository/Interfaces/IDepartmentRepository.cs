using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Students;

namespace SMSystem.Repository.Interfaces
{
    public interface IDepartmentRepository
    {
        List<DepartmentViewModel> GetAllDepartments();
        Task<DepartmentPaggedViewModel> GetDepartmnets(SearchingParaModel para);
        Task<DepartmentViewModel> GetDepartment(int id);
        Task<bool> Add(DepartmentViewModel department);
        Task<bool> Update(DepartmentViewModel department);
        Task<bool> Delete(int id);
    }
}
