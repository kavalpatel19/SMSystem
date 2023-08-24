using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Exam;

namespace SMSystem_Api.Model.Fees
{
    public class PaggedFeesModel
    {
        public PaggedFeesModel()
        {
            FeesModel = new List<FeesModel>();
            PaggedModel = new PaggedModel();
        }
        public IList<FeesModel> FeesModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
