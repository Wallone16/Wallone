using FluentValidation;
using Wallone.Auth.Domain.Shared.Users.Constants;
using Wallone.Auth.Services.Contracts.Users.Commands;

namespace Wallone.Identity.Application.Validators
{
    internal sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(UserConstants.UserNameMaxLength)
                .WithMessage("Некорректное имя");
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Некорректный Email");
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage("Некорректный пароль");
            RuleFor(x => x.PasswordConfirm)
                .Equal(x => x.Password)
                .WithMessage("Пароли не совпадают");
            RuleFor(x => x.Password)
                .MaximumLength(UserConstants.PasswordMaxLength)
                .WithMessage("Максимальная длина пароля 100 символов");
        }
    }
}