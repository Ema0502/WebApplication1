using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Servicies;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            try
            {
                var listUsers = await _usersService.GetUsers();
                if (listUsers == null)
                {
                    return BadRequest();
                }
                return Ok(listUsers);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Error in the server" });
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            return await _usersService.GetUser(id);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> PutUser(Guid id, User user)
        {
            return await _usersService.PutUser(id, user);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            return await _usersService.PostUser(user);
        }

        //POST: api/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> PostLogin(Login user)
        {
            return await _usersService.PostLogin(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDTO>> DeleteUser(Guid id)
        {
            return await _usersService.DeleteUser(id);
        }
    }
}
