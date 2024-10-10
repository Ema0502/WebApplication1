using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f55afa6"),
                    UserName = "messias",
                    FirstName = "lio",
                    LastName = "messi",
                    Birth = DateTime.Parse("1987-06-27T17:38:10.548Z"),
                    Email = "messi@word.com",
                    Password = "admin123",
                    Role = "admin"
                }
            );
        }
    }
}
