using MediatR;
using Wallone.Auth.Services.Contracts.Users.Dto;
using Wallone.Shared.Contracts;

namespace Wallone.Auth.Services.Contracts.Users.Commands
{
    public sealed class RegisterUserCommand : IRequest<Result<UserDto>>
    {
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string PasswordConfirm { get; init; }
    }
}