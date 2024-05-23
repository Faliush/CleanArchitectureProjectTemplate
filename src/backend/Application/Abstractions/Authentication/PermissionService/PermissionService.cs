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
            .Include(x => x.Role)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .Select(x => x.Role)
            .ToArrayAsync();

        var permissions = roles
            .Select(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();

        return permissions;
    }
}
