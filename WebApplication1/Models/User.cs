namespace WebApplication1.Models
{
  public class User
  {
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birth { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
  }
}
