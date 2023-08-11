using System.ComponentModel.DataAnnotations;

namespace SMSystem.Models.Fees
{
    public class FeesViewModel : BaseEntityViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FeesId { get; set; }
        [Required]
        public string FeesType { get; set; }
        [Required]
        public string Class { get; set; }
        [Required]
        public int FeesAmount { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }
    }
}
