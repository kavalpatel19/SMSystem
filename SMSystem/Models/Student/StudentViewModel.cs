using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSystem.Models.Students
{
    public class StudentViewModel : BaseEntityViewModel
    {
        [Key]
        public int Id { get; set; }
        public string StudentId { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "The First Name is required.")]
        public string FirstName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "The Parent Name is required.")]
        public string ParentName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "The Last Name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The Gender is required to select.")]
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The Date of Birth is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "The Roll No is required.")]
        public string RollNo { get; set; }
        [Required(ErrorMessage = "The Blood Group is required to select.")]
        public string BloodGroup { get; set; }
        [Required(ErrorMessage = "The Religion is required to select.")]
        public string Religion { get; set; }
        [Required(ErrorMessage = "The Class is required to select.")]
        public string Class { get; set; }
        [Required(ErrorMessage = "The Section is required to select.")]
        public string Section { get; set; }
        [Required(ErrorMessage = "The Admission Id is required.")]
        public string AdmissionId { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "The Phone is required.")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid phone number.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "The Address is required.")]
        public string Address { get; set; }
        public string? Path { get; set; }
        [NotMapped]
        [Display(Name = "Choose Image")]
        public IFormFile? ImagePath { get; set; }
    }
}

