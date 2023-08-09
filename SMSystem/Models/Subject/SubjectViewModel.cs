using System.ComponentModel.DataAnnotations;

namespace SMSystem.Models.Subject
{
    public class SubjectViewModel : BaseEntityViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SubjectId { get; set; }
        [Required]
        public string SubjectName { get; set; }
        [Required]
        public string Class { get; set; }
    }
}
