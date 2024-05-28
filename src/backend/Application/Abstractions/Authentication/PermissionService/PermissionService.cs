using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Authentication.PermissionService;

public class PermissionService(ApplicationDbContext context)
    : IPermissionService
{
    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        var roles = await context
            .Set<User>()
            .Include(x => x.Roles)
            .Where(x => x.Id == userId)
            .Select(x => x.Roles)
            .ToArrayAsync();

        var permissions = roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.ToString())
            .ToHashSet();

        return permissions;
    }
}
