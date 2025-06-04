using System.ComponentModel.DataAnnotations;

namespace nutrigoal_backend.Models
{
    public class CreateProfileDto
    {
        [Range(30,500, ErrorMessage = "Weight must be between 30 and 500 kg.")]
        public float Weight { get; set; }
        [Range(20, 250, ErrorMessage = "Height must be between 20 and 250 cm.")]
        public float Height { get; set; }
        [Required(ErrorMessage = "Goal is required.")]
        public string Goal { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public char Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
