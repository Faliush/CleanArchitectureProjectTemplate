using FluentValidation;

namespace Application.Users.Commands.AddRoles;

public sealed class AddRolesToUserValidator : AbstractValidator<AddRolesToUserCommand>
{
    public AddRolesToUserValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.RoleIds).NotEmpty();
    }
}
