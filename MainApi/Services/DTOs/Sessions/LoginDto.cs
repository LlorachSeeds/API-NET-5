using System.ComponentModel.DataAnnotations;

namespace Services.DTOs.Sessions
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}