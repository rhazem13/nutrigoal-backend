using System.ComponentModel.DataAnnotations;

namespace nutrigoal_backend.Models
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username must be between 3 and 50 characters.", MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email must be a maximum of 100 characters.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(256, ErrorMessage = "Password must be between 6 and 256 characters.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
