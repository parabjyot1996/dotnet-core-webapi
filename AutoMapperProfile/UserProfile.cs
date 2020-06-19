using AutoMapper;
using NetCoreAPI.Dtos;
using NetCoreAPI.Models;

namespace NetCoreAPI.AutoMapperProfile
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}