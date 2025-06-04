using System.ComponentModel.DataAnnotations;

namespace nutrigoal_backend.Models
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        [StringLength(100, ErrorMessage = "Username or Email must be a maximum of 100 characters.")]
        public string UserNameOrEmail { get; set; } = string.Empty;
        // Required: Password must be provided for login
        [Required(ErrorMessage = "Password is required.")]

        public string Password { get; set; } = string.Empty;
    }
}
