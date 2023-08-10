using System.ComponentModel.DataAnnotations;

namespace SMSystem_Api.Model.Holiday
{
    public class HolidayModel : BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
        public string HolidayId { get; set; }
        public string HolidayName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string HolidayType { get; set; }
    }
}
