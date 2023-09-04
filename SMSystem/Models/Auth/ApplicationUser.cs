using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SMSystem.Models.Auth
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string RepeatPassword { get; set; }
        [Required]
        [Remote(action: "EmailExist", controller: "Account", AdditionalFields = "Id")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Not a valid e-mail address.")]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
