using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ActionResult<IEnumerable<User>>> GetUsers();
        Task<ActionResult<User>> GetUser(Guid id);
        Task<ActionResult<User>> PutUser(Guid id, User user);
        Task<ActionResult<User>> PostUser(User user);
        Task<ActionResult<User>> PostLogin(Login user);
        Task<ActionResult<User>> DeleteUser(Guid id);
  }
}
