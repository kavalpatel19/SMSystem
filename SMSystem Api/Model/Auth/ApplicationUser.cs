using System.ComponentModel.DataAnnotations;

namespace SMSystem_Api.Model.Auth
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
