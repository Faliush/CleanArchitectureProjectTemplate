using FluentValidation;

namespace Application.Users.Commands.SetRoles;

public sealed class SetRolesToUserValidator : AbstractValidator<SetRolesToUserCommand>
{
    public SetRolesToUserValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty();

        RuleFor(x => x.RoleIds).NotNull().NotEmpty();
    }
}
