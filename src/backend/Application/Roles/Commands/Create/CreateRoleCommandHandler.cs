using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Application.Roles.Commands.Create;

internal sealed class CreateRoleCommandHandler(
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var permissions = await permissionRepository.GetAllAsync(
            predicate: x => request.PermissionIds.Contains(x.Id), 
            cancellationToken: cancellationToken);
        
        var role = new Role { Name = request.Name, Permissions = permissions };

        await roleRepository.InsertAsync(role, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
