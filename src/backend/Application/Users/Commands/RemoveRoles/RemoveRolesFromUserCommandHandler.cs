using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.RemoveRoles;

internal sealed class RemoveRolesFromUserCommandHandler(
    UserManager<User> userManager,
    RoleManager<Role> roleManager) 
    : ICommandHandler<RemoveRolesFromUserCommand>
{
    public async Task<Result> Handle(RemoveRolesFromUserCommand request, CancellationToken cancellationToken)
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

        await userManager.RemoveFromRolesAsync(user, roleNames!);

        return Result.Success();
    }
}
