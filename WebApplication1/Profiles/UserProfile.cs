using AutoMapper;
using WebApplication1.Models;
using WebApplication1.Dtos;


namespace WebApplication1.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Login, LoginDTO>();
            CreateMap<LoginDTO, Login>();
        }
    }
}