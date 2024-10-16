namespace WebApplication1.Dtos
{
  public class UserDTO
  {
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birth { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
  }
}
