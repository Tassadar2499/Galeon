using FluentValidation;
using GaleonServer.Models.Commands;

namespace GaleonServer.Core.Validation;

public class RegisterCommandValidator: AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(z => z.Email).EmailAddress().NotEmpty();
        RuleFor(z => z.Password).NotEmpty();
        RuleFor(z => z.PasswordConfirm).NotEmpty().Equal(z => z.Password).WithErrorCode("Пароли не совпадают");
    }
}