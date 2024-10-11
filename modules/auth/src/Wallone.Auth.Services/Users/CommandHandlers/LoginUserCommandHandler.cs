using AutoMapper;
using FluentValidation;
using MediatR;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.Services.Contracts.Users;
using Wallone.Auth.Services.Contracts.Users.Commands;
using Wallone.Auth.Services.Contracts.Users.Dto;
using Wallone.Shared.Domain;

namespace Wallone.Auth.Services.Users.CommandHandlers
{
    internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IValidator<LoginUserCommand> _validator;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IHashPasswordService hashPasswordService,
            IValidator<LoginUserCommand> validator,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return new Result<UserDto>
                {
                    ErrorMessages = new List<string>(validatorResult.Errors
                        .Select(x => x.ErrorMessage))
                };
            }

            var existingUserResult = await _userRepository
                .GetUserByEmailAsync(request.Email);

            var user = existingUserResult;

            if (user == null)
            {
                return new Result<UserDto>
                {
                    ErrorMessages = new List<string> { "Неверный логин или пароль" }
                };
            }

            bool isEqualsPassword = _hashPasswordService.IsVerifyPassword(request.Password, user.Password);

            if (!isEqualsPassword)
            {
                return new Result<UserDto>
                {
                    ErrorMessages = new List<string> { "Неверный логин или пароль" }
                };
            }

            return new Result<UserDto>
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
    }
}