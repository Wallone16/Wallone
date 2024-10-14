using AutoMapper;
using MediatR;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.Services.Contracts.Users.Dto;
using Wallone.Auth.Services.Contracts.Users.Queries;
using Wallone.Shared.Contracts;

namespace Wallone.Auth.Services.Users.QueryHandlers
{
    internal sealed class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public GetUserByEmailQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Task<Result<UserDto>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}