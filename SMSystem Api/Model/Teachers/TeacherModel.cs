using System.ComponentModel.DataAnnotations;

namespace SMSystem_Api.Model.Teachers
{
    public class TeacherModel : BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
        public string TeacherId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNo { get; set; }
        public string Subject { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Qualification { get; set; }
        public string Experience { get; set; }
        public string? Path { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
