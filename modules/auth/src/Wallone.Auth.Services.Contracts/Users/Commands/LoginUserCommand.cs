using MediatR;
using Wallone.Auth.Services.Contracts.Users.Dto;
using Wallone.Shared.Contracts;

namespace Wallone.Auth.Services.Contracts.Users.Commands
{
    public sealed class LoginUserCommand : IRequest<Result<UserDto>>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}