using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Servicies
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<ActionResult<UserDTO>> GetUser(Guid id);
        Task<ActionResult<UserDTO>> PutUser(Guid id, User user);
        Task<ActionResult<UserDTO>> PostUser(User user);
        Task<ActionResult<LoginDTO>> PostLogin(Login user);
        Task<ActionResult<UserDTO>> DeleteUser(Guid id);
    }
}
