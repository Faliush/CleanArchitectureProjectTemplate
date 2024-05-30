using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Roles.Queries.GetAll;

internal sealed class GetAllRolesQueryHandler(RoleManager<Role> roleManager)
    : IQueryHandler<GetAllRolesQuery, Result>
{
    public async Task<Result> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles
            .Select(x => new RoleResponse 
            { 
                Id = x.Id, 
                Name = x.Name, 
                Permissions = x.Permissions
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return Result.Success(roles);
    }
}
