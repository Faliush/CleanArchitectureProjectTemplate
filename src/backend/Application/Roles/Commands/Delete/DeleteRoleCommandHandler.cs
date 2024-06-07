using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Roles.Commands.Delete;

internal sealed class DeleteRoleCommandHandler(
    RoleManager<Role> roleManager) 
    : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id.ToString());

        if(role is null)
        {
            return Result.Failure(DomainErrors.Role.NotFound);
        }

        await roleManager.DeleteAsync(role);

        return Result.Success();
    }
}
