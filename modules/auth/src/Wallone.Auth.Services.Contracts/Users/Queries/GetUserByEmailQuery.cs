using MediatR;
using Wallone.Auth.Services.Contracts.Users.Dto;
using Wallone.Shared.Contracts;

namespace Wallone.Auth.Services.Contracts.Users.Queries
{
    public sealed class GetUserByEmailQuery : IRequest<Result<UserDto>>
    {
        public required string Email { get; init; }
    }
}