using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMSystem.Models.Department
{
    public class DepartmentViewModel : BaseEntityViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Department Id")]
        public string DepartmentId { get; set; }
        [Required]
        [Display(Name ="Department Name")]
        public string DepartmentName { get; set; }
        [Required]
        [Display(Name = "Head Of Department")]
        public string HeadOfDepartment { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Started Year")]
        public DateTime StartedYear { get; set; }
        [Required]
        [Display(Name = "No Of Students")]
        public int NoOfStudents { get; set; }
    }
}
