using Microsoft.EntityFrameworkCore;
using nutrigoal_backend.Models.Entities;

namespace nutrigoal_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<Meal> Meals { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(u=>u.Username).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            });

            // Configure Profile
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p=>p.UserId).IsRequired();
                entity.Property(p => p.Weight).IsRequired();
                entity.Property(p => p.Height).IsRequired();
                entity.Property(p => p.DateOfBirth).IsRequired();
                entity.Property(p => p.Goal).IsRequired().HasMaxLength(50);
                entity.Property(p => p.CaloricTarget).IsRequired();
                entity.Property(p => p.ProteinTarget).IsRequired();
                entity.Property(p => p.CarbTarget).IsRequired();
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(50);
                entity.HasOne(p => p.User)
                    .WithOne()
                    .HasForeignKey<Profile>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Meal
            modelBuilder.Entity<Meal>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.UserId).IsRequired();
                entity.Property(m => m.Date).IsRequired().HasDefaultValueSql("GETUTCDATE()");
                entity.Property(m => m.FoodName).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Calories).IsRequired();
                entity.Property(m => m.Protein).IsRequired();
                entity.Property(m => m.Carbs).IsRequired();
                entity.Property(m => m.Description).HasMaxLength(500);
                entity.HasOne(m => m.User)
                    .WithMany()
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
