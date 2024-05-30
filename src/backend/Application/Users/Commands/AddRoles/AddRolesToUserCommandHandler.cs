using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.AddRoles;

internal sealed class AddRolesToUserCommandHandler(
    UserManager<User> userManager,
    RoleManager<Role> roleManager) 
    : ICommandHandler<AddRolesToUserCommand, Result>
{
    public async Task<Result> Handle(AddRolesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if(user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        var roleNames = await roleManager.Roles
            .Where(x => request.RoleIds.Contains(x.Id))
            .Select(x => x.Name)
            .ToListAsync(cancellationToken: cancellationToken);

        if(roleNames is null)
        {
            return Result.Failure(DomainErrors.Role.NotFound);
        }

        var addedToRoleResult = await userManager.AddToRolesAsync(user, roleNames!);

        if (!addedToRoleResult.Succeeded)
        {
            return Result.Failure(new Error(
                addedToRoleResult.Errors.First().Code,
                addedToRoleResult.Errors.First().Description));
        }

        return Result.Success();
    }
}
