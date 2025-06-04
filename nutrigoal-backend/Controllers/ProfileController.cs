using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nutrigoal_backend.Data;
using nutrigoal_backend.Models;
using nutrigoal_backend.Models.Entities;

namespace nutrigoal_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Profile
        // Retrieves the profile of the authenticated user
        [HttpGet]
        public IActionResult GetProfile()
        {
            // TODO: Replace wth JWT authentication to get user ID 
            var userId = 1; // Harcoded for development purposes, replace with User.FindFirstValue(ClaimTypes.NameIdentifier) 
            var profile = _context.Profiles.AsNoTracking().FirstOrDefault(p => p.UserId == userId);
            if (profile == null)
            {
                return NotFound("Profile not found.");
            }
            return Ok(profile);
        }

        // POST: api/Profile
        // Creates or updates a user's profile with calculated caloric and macronutrient targets
        [HttpPost]
        public IActionResult SaveProfile([FromBody] CreateProfileDto profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Replace with JWT authentication to get user ID
            var userId = 1; // Hardcoded for development 

            // Validate goal
            var validGoals = new[] { "bulking", "cutting", "maintaining" };
            if (string.IsNullOrWhiteSpace(profile.Goal) || !validGoals.Contains(profile.Goal.ToLower()))
            {
                return BadRequest("Goal must be one of: bulking, cutting, maintaining.");
            }

            // Calculate age
            var today = DateTime.Today;
            var age = today.Year - profile.DateOfBirth.Year;
            if (profile.DateOfBirth.Date > today.AddYears(-age)) age--;

            // Mifflin-St Jeor Equation (gender-based)
            double bmr;
            if (profile.Gender == 'F' || profile.Gender == 'f')
            {
                bmr = 10 * profile.Weight + 6.25 * profile.Height - 5 * age - 161;
            }
            else // Default to male if not 'F'
            {
                bmr = 10 * profile.Weight + 6.25 * profile.Height - 5 * age + 5;
            }
            var caloricTarget = bmr * 1.5f; // Moderate activity

            // Adjust for goal
            switch (profile.Goal.ToLower())
            {
                case "bulking":
                    caloricTarget *= 1.10f; // +10%
                    break;
                case "cutting":
                    caloricTarget *= 0.90f; // -10%
                    break;
                    // maintaining: no adjustment
            }

            // Macronutrient targets (grams)
            float proteinPercent, carbPercent;
            if (profile.Goal.ToLower() == "bulking")
            {
                carbPercent = 0.40f;
                proteinPercent = 0.30f;
            }
            else if (profile.Goal.ToLower() == "cutting")
            {
                carbPercent = 0.30f;
                proteinPercent = 0.40f;
            }
            else // maintaining
            {
                carbPercent = 0.35f;
                proteinPercent = 0.35f;
            }

            // 1g protein = 4 kcal, 1g carb = 4 kcal
            var proteinTarget = (caloricTarget * proteinPercent) / 4f;
            var carbTarget = (caloricTarget * carbPercent) / 4f;

            // Map DTO to entity
            var existingProfile = _context.Profiles.FirstOrDefault(p => p.UserId == userId);
            if (existingProfile == null)
            {
                existingProfile = new Profile
                {
                    UserId = userId
                };
                _context.Profiles.Add(existingProfile);
            }

            existingProfile.FirstName = profile.FirstName;
            existingProfile.LastName = profile.LastName;
            existingProfile.Weight = profile.Weight;
            existingProfile.Height = profile.Height;
            existingProfile.Goal = profile.Goal;
            existingProfile.DateOfBirth = profile.DateOfBirth;
            existingProfile.CaloricTarget = (float)caloricTarget;
            existingProfile.ProteinTarget = (float)proteinTarget;
            existingProfile.CarbTarget = (float)carbTarget;
            existingProfile.Gender = profile.Gender;

            _context.SaveChanges();

            return Ok(existingProfile);
        }
    }
}
