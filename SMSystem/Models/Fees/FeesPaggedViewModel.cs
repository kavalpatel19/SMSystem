using SMSystem.Models.Department;

namespace SMSystem.Models.Fees
{
    public class FeesPaggedViewModel
    {
        public FeesPaggedViewModel()
        {
            FeesModel = new List<FeesViewModel>();
            PaggedModel = new PaggedViewModel();
        }
        public IList<FeesViewModel> FeesModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
