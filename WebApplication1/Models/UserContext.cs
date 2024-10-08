using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace YourApi.Models
{
  public class UserContext : DbContext
  {
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
  }
}
