using AutoMapper;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.Services.Contracts.Users.Dto;

namespace Wallone.Auth.Services
{
    internal sealed class AuthModuleMappings : Profile
    {
        public AuthModuleMappings()
        {
            CreateMap<User, UserDto>();
        }
    }
}
