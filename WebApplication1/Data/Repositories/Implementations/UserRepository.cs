using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                return await _context.Users
                    .Select(user => user)
                    .ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error getting users in database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return new NotFoundResult();
                }

                return user;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error getting user in database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
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
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error creating a user in database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<ActionResult<User>> PostLogin(Login user)
        {
            try
            {
                var existingUser = await _context.Users
                      .FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

                if (existingUser == null)
                {
                    return new NoContentResult();
                }

                return existingUser;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error getting data in database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }


        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            try
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
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error getting user in database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
