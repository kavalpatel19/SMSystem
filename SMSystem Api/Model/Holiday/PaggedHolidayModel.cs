
using SMSystem_Api.Model.Fees;

namespace SMSystem_Api.Model.Holiday
{
    public class PaggedHolidayModel
    {
        public PaggedHolidayModel()
        {
            HolidayModel = new List<HolidayModel>();
            PaggedModel = new PaggedModel();
        }
        public IList<HolidayModel> HolidayModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
