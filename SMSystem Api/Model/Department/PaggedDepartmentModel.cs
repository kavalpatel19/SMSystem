using SMSystem_Api.Model.Students;

namespace SMSystem_Api.Model.Department
{
    public class PaggedDepartmentModel
    {
        public PaggedDepartmentModel()
        {
            DepartmentModel = new List<DepartmentModel>();
            PaggedModel = new PaggedModel();
        }
        public List<DepartmentModel> DepartmentModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
