using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSystem_Api.Model.Auth
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string RepeatPassword { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
    }
}
