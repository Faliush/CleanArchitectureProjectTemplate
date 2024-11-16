using Application.Abstractions.Messaging;
using Application.Roles.Queries.GetAllPermissions;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Roles.Queries.GetAll;

internal sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleFullResponse>>
{
    public async Task<Result<IEnumerable<RoleFullResponse>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(
            disableTracking: true, 
            include: i => i.Include(x => x.Permissions),
            selector: x =>
                new RoleFullResponse(x.Id, x.Name, x.Permissions
                    .Select(p => new PermissionResponse(p.Id, p.Name))), cancellationToken: cancellationToken);

        return roles.ToList();
    }
}
