using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Students;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IDepartmentApiRepository
    {
        BaseResponseModel<DepartmentModel> GetAllDepartments();
        Task<BaseResponseModel<PaggedDepartmentModel>> GetAll(SearchingPara para);
        Task<BaseResponseModel<DepartmentModel>> Get(int id);
        Task<BaseResponseModel<DepartmentModel>> Add(DepartmentModel student);
        Task<BaseResponseModel<DepartmentModel>> Update(DepartmentModel student);
        Task<BaseResponseModel<DepartmentModel>> Delete(int id);
    }
}
