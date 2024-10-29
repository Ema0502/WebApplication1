using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Data.Repositories.Interfaces;

namespace WebApplication1.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Select(user => user)
                .ToListAsync();
        }

        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return new NotFoundResult();
            }

            return user;
        }

        public async Task<ActionResult<User>> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return new NotFoundResult();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return await this.GetUser(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ActionResult<User>> PostLogin(Login user)
        {
            var existingUser = await _context.Users
                  .FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

            if (existingUser == null)
            {
                return new NoContentResult();
            }

            return existingUser;
        }

        
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private static UserDTO UserToDTO(User user) =>
            new UserDTO
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birth = user.Birth,
                Email = user.Email,
                Role = user.Role,
            };
    }
}
