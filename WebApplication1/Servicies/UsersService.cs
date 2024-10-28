using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Repositories.Interfaces;
using WebApplication1.Dtos;
using WebApplication1.Models;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data.Repositories.Implementations;
using WebApplication1.Servicies;

namespace WebApplication1.Servicies
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _secretKey;
        public UsersService(IConfiguration config, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _secretKey = config.GetSection("Settings").GetSection("SecretKey").ToString();
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
            var userExister = await _userRepository.PostLogin(user);

            if (userExister == null)
            {
                return new UnauthorizedResult();
            }

            var token = GenerateJwtToken(userExister.Email, userExister.Role);

            return new LoginDTO
            {
                Email = userExister.Email,
                Role = userExister.Role,
                Access = true,
                Token = token
            };
        }

        public async Task<IActionResult> DeleteUser(Guid id)
        {
            return await _userRepository.DeleteUser(id);
        }

        private string GenerateJwtToken(string email, string role)
        {
            var keyBytes = System.Text.Encoding.ASCII.GetBytes(_secretKey);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, email));
            claims.AddClaim(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tokenConfig);
        }
    }
}
