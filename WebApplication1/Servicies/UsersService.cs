using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Repositories.Interfaces;
using WebApplication1.Dtos;
using WebApplication1.Models;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
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
            try
            {
                var listUsers = await _userRepository.GetUsers();
                if (listUsers == null)
                {
                    throw new ArgumentException("No users found in the repository");
                }
                return _mapper.Map<IEnumerable<UserDTO>>(listUsers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            try
            {
                var user = await _userRepository.GetUser(id);
                if (user == null)
                {
                    throw new ArgumentException("No user found in the repository");
                }
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ActionResult<UserDTO>> PutUser(Guid id, User user)
        {
            try
            {
                var editUser = await _userRepository.PutUser(id, user);
                if (editUser == null)
                {
                    throw new ArgumentException("No user found in the repository");
                }
                return _mapper.Map<UserDTO>(editUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            try
            {
                var createUser = await _userRepository.PostUser(user);
                if (user == null)
                {
                    throw new ArgumentException("Error in the repository");
                }
                return _mapper.Map<UserDTO>(createUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ActionResult<LoginDTO>> PostLogin(Login user)
        {
            try
            {
                var userExister = await _userRepository.PostLogin(user);

                if (userExister.Value == null)
                {
                    return new UnauthorizedResult();
                }

                var userDto = _mapper.Map<UserDTO>(userExister.Value);
                var token = this.GenerateJwtToken(userDto.Email, userDto.Role);

                LoginDTO loginDto = new LoginDTO
                {
                    Email = userDto.Email,
                    Role = userDto.Role,
                    Access = true,
                    Token = token
                };
                return loginDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ActionResult<UserDTO>> DeleteUser(Guid id)
        {
            try
            {
                var deleteUser = await _userRepository.DeleteUser(id);
                if (deleteUser == null)
                {
                    throw new ArgumentException("No user found in the repository");
                }
                return _mapper.Map<UserDTO>(deleteUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private string GenerateJwtToken(string email, string role)
        {
            var keyBytes = System.Text.Encoding.ASCII.GetBytes(_secretKey);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, email));
            claims.AddClaim(new Claim(ClaimTypes.Name, role));

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
