using SMSystem_Api.Model.Students;

namespace SMSystem_Api.Model.Department
{
    public class PaggedDepartmentModel
    {
        public IList<DepartmentModel> DepartmentModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
