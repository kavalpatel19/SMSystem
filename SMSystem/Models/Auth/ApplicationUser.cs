using System.ComponentModel.DataAnnotations;

namespace SMSystem.Models.Auth
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string RepeatPassword { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
