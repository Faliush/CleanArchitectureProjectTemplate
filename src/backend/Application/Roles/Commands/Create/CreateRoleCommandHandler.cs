using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Roles.Commands.Create;

internal sealed class CreateRoleCommandHandler(
    RoleManager<Role> roleManager)
    : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role { Name = request.Name, Permissions = request.Permissions };

        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            return Result.Failure(DomainErrors.Role.CannotCreateRole);
        }

        return Result.Success();
    }
}
