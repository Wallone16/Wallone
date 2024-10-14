using AutoMapper;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.Services.Contracts.Users.Dto;

namespace Wallone.Auth.AutoMapper.Users
{
    internal sealed class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}