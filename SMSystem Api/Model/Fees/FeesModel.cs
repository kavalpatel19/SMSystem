using System.ComponentModel.DataAnnotations;

namespace SMSystem_Api.Model.Fees
{
    public class FeesModel : BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
        public string FeesId { get; set; }
        public string FeesType { get; set; }
        public string Class { get; set; }
        public int FeesAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}