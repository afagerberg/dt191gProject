using dt191gProjectApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dt191gProjectApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<Booking>()
                .HasOne(b => b.Workout).WithMany(w => w.Bookings);

            _ = modelBuilder.Entity<Workout>()
                .HasOne(w => w.TypeOfWorkout).WithMany(t => t.Workouts);

        }

        public DbSet<TypeOfWorkout>? TypeOfWorkout { get; set; }
        public DbSet<Workout>? Workout { get; set; }
        public DbSet<Booking>? Booking { get; set; }
    }
}
