namespace nutrigoal_backend.Models.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public string Goal { get; set; } = string.Empty;
        public float CaloricTarget { get; set; }
        public float ProteinTarget { get; set; }
        public float CarbTarget { get; set; }

        public User User { get; set; } = null!;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
