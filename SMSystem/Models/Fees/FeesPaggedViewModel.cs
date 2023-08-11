namespace SMSystem.Models.Fees
{
    public class FeesPaggedViewModel
    {
        public IList<FeesViewModel> FeesModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
