
namespace SMSystem_Api.Model.Holiday
{
    public class PaggedHolidayModel
    {
        public IList<HolidayModel> HolidayModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
