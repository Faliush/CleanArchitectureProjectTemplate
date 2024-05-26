using Application.Abstractions.Messaging;
using Application.Roles.Queries.GetAllPermissions;
using Domain.Core.Primitives.Result;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Roles.Queries.GetAll;

internal sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetAllRolesQuery, Result>
{
    public async Task<Result> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(
            include: i => i.Include(x => x.Permissions!),
            disableTracking: true,
            disableQuerySpliting: false,
            selector: s => new RoleResponse
            {
                Id = s.Id,
                Name = s.Name,
                Permissions = s.Permissions!.Select(x => new PermissionResponse
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            }, 
            cancellationToken: cancellationToken);

        return Result.Success(roles);
    }
}
