using FluentValidation;
using Wallone.Auth.Services.Contracts.Users.Commands;

namespace Wallone.Identity.Application.Validators
{
    internal sealed class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Некорректный Email");
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage("Некорректный пароль");
        }
    }
}