using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nutrigoal_backend.Data;
using nutrigoal_backend.Models;
using nutrigoal_backend.Models.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace nutrigoal_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/users/register
        // Registers a new user with username, email, and password
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationDto registrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Check if username or email already exists
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Username == registrationDto.Username || u.Email == registrationDto.Email);
            if (existingUser != null)
            {
                return Conflict("Username or email already exists.");
            }
            // Create new user
            var user = new User
            {
                Username = registrationDto.Username,
                Email = registrationDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password)
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        // POST: api/users/login
        // Authenticates a user and returns a JWT token
        //[HttpPost("login")]
        //public IActionResult Login([FromBody] UserLoginDto loginDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    // Find user by email or username
        //    var user = _context.Users
        //        .FirstOrDefault(u => u.Username == loginDto.UserNameOrEmail || u.Email == loginDto.UserNameOrEmail);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        //        {
        //            return Unauthorized("Invalid username/email or password.");
        //    }

        //    // Generate JWT token
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);


        //}

    }
}
