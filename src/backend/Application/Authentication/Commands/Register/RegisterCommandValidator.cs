using Application.Abstractions.Validation;
using FluentValidation;

namespace Application.Authentication.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(ValidationRules.User.FirstNameMaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(ValidationRules.User.LastNameMaxLength);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(ValidationRules.User.EmailMaxLength)
            .Matches(ValidationRules.User.EmailRegexPattern);

        RuleFor(x => x.Password).NotEmpty();
    }
}
