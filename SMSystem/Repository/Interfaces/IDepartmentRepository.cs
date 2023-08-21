using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Students;

namespace SMSystem.Repository.Interfaces
{
    public interface IDepartmentRepository
    {
        BaseResponseViewModel<DepartmentViewModel> GetAllDepartments();
        Task<BaseResponseViewModel<DepartmentPaggedViewModel>> GetDepartmnets(SearchingParaModel para);
        Task<BaseResponseViewModel<DepartmentViewModel>> GetDepartment(int id);
        Task<bool> Add(DepartmentViewModel department);
        Task<bool> Update(DepartmentViewModel department);
        Task<bool> Delete(int id);
    }
}
