using SMSystem_Api.Model.Department;

namespace SMSystem_Api.Model.Fees
{
    public class PaggedFeesModel
    {
        public IList<FeesModel> FeesModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
