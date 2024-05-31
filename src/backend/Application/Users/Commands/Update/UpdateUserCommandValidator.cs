using Application.Abstractions.Validation;
using FluentValidation;

namespace Application.Users.Commands.Update;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.FirstName)
           .NotEmpty()
           .MaximumLength(ValidationRules.User.FirstNameMaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(ValidationRules.User.LastNameMaxLength);
    }
}
