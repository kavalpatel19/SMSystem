using System.ComponentModel.DataAnnotations;

namespace SMSystem.Models.Holiday
{
    public class HolidayViewModel : BaseEntityViewModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Holiday Id")]
        public string HolidayId { get; set; }
        [Required]
        [Display(Name ="Holiday Name")]
        public string HolidayName { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "Holiday Type")]
        public string HolidayType { get; set; }
    }
}
