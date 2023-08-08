using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSystem.Models.Teacher
{
    public class TeacherViewModel : BaseEntityViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Teacher Id")]
        public string TeacherId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name="Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Display(Name= "Phone No")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid phone number.")]
        public string PhoneNo { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime JoiningDate { get; set; }
        [Required]
        public string Qualification { get; set; }
        [Required]
        public string Experience { get; set; }
        public string? Path { get; set; }
        [NotMapped]
        [Display(Name = "Choose Image")]
        public IFormFile? ImagePath { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
