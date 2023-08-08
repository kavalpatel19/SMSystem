using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Students;

namespace SMSystem_Api.Repository
{
    public interface IDepartmentApiRepository
    {
        List<DepartmentModel> GetAllDepartments();
        Task<PaggedDepartmentModel> GetAll(SearchingPara para);
        Task<DepartmentModel> Get(int id);
        Task Add(DepartmentModel student);
        Task Update(DepartmentModel student);
        Task Delete(int id);
    }
}
