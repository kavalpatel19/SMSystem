using System.ComponentModel.DataAnnotations;

namespace SMSystem.Models.Auth
{
    public class PasswordModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "Please Enter Old Password!")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Please Enter New Password!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage ="Please Confirm New Password!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
