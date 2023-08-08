using System.ComponentModel.DataAnnotations;

namespace SMSystem.Models.Subject
{
    public class SubjectViewModel : BaseEntityViewModel
    {
        [Key]
        public int Id { get; set; }
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Class { get; set; }
    }
}
