using FluentValidation;

namespace Application.Roles.Commands.Create;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Permissions).NotEmpty();
    }
}
