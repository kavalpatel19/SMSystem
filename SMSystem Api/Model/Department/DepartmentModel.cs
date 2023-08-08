using System.ComponentModel.DataAnnotations;

namespace SMSystem_Api.Model.Department
{
    public class DepartmentModel:BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string HeadOfDepartment { get; set; }
        public DateTime StartedYear { get; set; }
        public int NoOfStudents { get; set; }
    }
}
