using MediatR;
using Wallone.Auth.Services.Contracts.Users.Dto;
using Wallone.Shared.Domain;

namespace Wallone.Auth.Services.Contracts.Users.Queries
{
    public sealed class GetUserByEmailQuery : IRequest<Result<UserDto>>
    {
        public required string Email { get; init; }
    }
}