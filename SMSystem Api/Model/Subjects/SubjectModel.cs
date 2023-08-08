using System.ComponentModel.DataAnnotations;

namespace SMSystem_Api.Model.Subjects
{
    public class SubjectModel : BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Class { get; set; }
    }
}
