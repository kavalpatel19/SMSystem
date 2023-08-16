using System.ComponentModel.DataAnnotations;

namespace SMSystem_Api.Model.Exam
{
    public class ExamModel : BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
        public string ExamName { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ExamDate { get; set; }
    }
}
