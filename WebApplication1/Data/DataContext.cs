using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(user => user.Id);

            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .HasKey(product => product.Id);

            modelBuilder.Entity<Product>()
              .HasOne(product => product.User)
              .WithMany(user => user.Products)
              .HasForeignKey(product => product.UserId);

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