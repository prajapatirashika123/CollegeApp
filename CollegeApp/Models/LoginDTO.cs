using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{
    public class LoginDTO
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string PolicyName { get; set; }

        [Required]
        public string Username { get; set; }
    }
}