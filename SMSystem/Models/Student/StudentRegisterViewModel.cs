using SMSystem.Models.Auth;
using SMSystem.Models.Students;

namespace SMSystem.Models.Student
{
    public class StudentRegisterViewModel
    {
        public StudentViewModel StudentModel { get; set; }
        public ApplicationUser UserModel { get; set; }
    }
}
