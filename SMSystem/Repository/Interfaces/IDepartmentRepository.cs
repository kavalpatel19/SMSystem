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
        Task<BaseResponseViewModel<DepartmentViewModel>> Add(DepartmentViewModel department);
        Task<BaseResponseViewModel<DepartmentViewModel>> Update(DepartmentViewModel department);
        Task<BaseResponseViewModel<DepartmentViewModel>> Delete(int id);
    }
}
