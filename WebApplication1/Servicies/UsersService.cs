using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Repositories.Interfaces;
using WebApplication1.Dtos;
using WebApplication1.Models;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data.Repositories.Implementations;
using WebApplication1.Servicies;
using AutoMapper;

namespace WebApplication1.Servicies
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _secretKey;
        private readonly IMapper _mapper;
        public UsersService(IConfiguration config, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _secretKey = config.GetSection("Settings").GetSection("SecretKey").ToString();
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var listUsers = await _userRepository.GetUsers();
            if (listUsers == null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<UserDTO>>(listUsers);
        }

        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            var user = await _userRepository.GetUser(id);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<ActionResult<UserDTO>> PutUser(Guid id, User user)
        {
            var editUser = await _userRepository.PutUser(id, user);
            if (editUser == null)
            {
                return new NoContentResult();
            }
            return _mapper.Map<UserDTO>(editUser);
        }

        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            var createUser = await _userRepository.PostUser(user);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(createUser);
        }

        public async Task<ActionResult<LoginDTO>> PostLogin(Login user)
        {
            var userExister = await _userRepository.PostLogin(user);

            if (userExister == null)
            {
                return new UnauthorizedResult();
            }

            var loginDto = _mapper.Map<LoginDTO>(userExister);
            loginDto.Token = GenerateJwtToken(loginDto.Email, loginDto.Role);
            loginDto.Access = true;

            return loginDto;
        }

        public async Task<ActionResult<UserDTO>> DeleteUser(Guid id)
        {
            var deleteUser = await _userRepository.DeleteUser(id);
            return _mapper.Map<UserDTO>(deleteUser);
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
