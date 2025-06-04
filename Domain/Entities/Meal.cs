namespace nutrigoal_backend.Models.Entities
{
    public class Meal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string FoodName { get; set; } = string.Empty;
        public float Calories { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public string Description { get; set; } = string.Empty;
        public User User{ get; set; } = null!;
    }
}
