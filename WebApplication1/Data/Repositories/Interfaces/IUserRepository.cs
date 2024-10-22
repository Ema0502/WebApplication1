using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ActionResult<IEnumerable<UserDTO>>> GetUsers();
        Task<ActionResult<UserDTO>> GetUser(Guid id);
        Task<IActionResult> PutUser(Guid id, User user);
        Task<ActionResult<User>> PostUser(User user);
        Task<ActionResult<LoginDTO>> PostLogin(Login user);
        Task<IActionResult> DeleteUser(Guid id);
    }
}
