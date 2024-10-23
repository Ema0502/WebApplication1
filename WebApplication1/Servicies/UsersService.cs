using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Repositories.Interfaces;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Servicies
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;

        public UsersService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            return await _userRepository.GetUser(id);
        }

        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            return await _userRepository.PutUser(id, user);
        }

        public async Task<ActionResult<User>> PostUser(User user)
        {
            return await _userRepository.PostUser(user);
        }

        public async Task<ActionResult<LoginDTO>> PostLogin(Login user)
        {
            return await _userRepository.PostLogin(user);
        }

        public async Task<IActionResult> DeleteUser(Guid id)
        {
            return await _userRepository.DeleteUser(id);
        }
    }
}
