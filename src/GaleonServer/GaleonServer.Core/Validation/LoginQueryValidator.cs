using FluentValidation;
using GaleonServer.Models.Queries;

namespace GaleonServer.Core.Validation;

public class LoginQueryValidator: AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(z => z.Email).EmailAddress().NotEmpty();
        RuleFor(z => z.Password).NotEmpty();
    }
}