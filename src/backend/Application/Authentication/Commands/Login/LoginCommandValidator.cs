using Application.Abstractions.Validation;
using FluentValidation;

namespace Application.Authentication.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(ValidationRules.User.EmailMaxLength)
            .Matches(ValidationRules.User.EmailRegexPattern);
        
        RuleFor(x => x.Password).NotEmpty();
    }
}
