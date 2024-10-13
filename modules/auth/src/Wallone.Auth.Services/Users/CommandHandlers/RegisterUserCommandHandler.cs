using AutoMapper;
using FluentValidation;
using MediatR;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.Services.Contracts.Users;
using Wallone.Auth.Services.Contracts.Users.Commands;
using Wallone.Auth.Services.Contracts.Users.Dto;
using Wallone.Shared.Contracts;
using Wallone.Shared.Services.Interfaces;

namespace Wallone.Auth.Services.Users.CommandHandlers
{
    internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserDto>>
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegisterUserCommand> _validator;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(
            IGuidGenerator guidGenerator,
            IUserRepository userRepository,
            IValidator<RegisterUserCommand> validator,
            IHashPasswordService hashPasswordService,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
        {
            _guidGenerator = guidGenerator;
            _userRepository = userRepository;
            _validator = validator;
            _hashPasswordService = hashPasswordService;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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
                .GetUserByEmailAsync(request.Email, cancellationToken);

            if (existingUserResult != null)
            {
                return new Result<UserDto>
                {
                    ErrorMessages = new List<string> { "Пользователь уже зарегистрирован" }
                };
            }

            var user = new User(
                id: _guidGenerator.Create(),
                userName: request.UserName,
                email: request.Email,
                password: _hashPasswordService.HashPassword(request.Password),
                roles: new List<Role> { Role.User });


            await _userRepository.InsertAsync(user, cancellationToken);

            return new Result<UserDto>
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
    }
}