using System.ComponentModel.DataAnnotations;

namespace SMSystem.Models.Exam
{
    public class ExamViewModel : BaseEntityViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Exam Name")]
        public string ExamName { get; set; }
        [Required]
        [Display(Name = "Class")]
        public string Class { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ExamDate { get; set; }
    }
}
