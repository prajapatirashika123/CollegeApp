using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{
    public class StudentDTO
    {
        [Required]
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; }
    }
}