using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSystem_Api.Model.Students
{
    public class StudentModel : BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string ParentName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RollNo { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string AdmissionId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Path { get; set; }
    }
}
