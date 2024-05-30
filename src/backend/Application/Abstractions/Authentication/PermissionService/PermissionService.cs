using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstractions.Authentication.PermissionService;

public class PermissionService(UserManager<User> userManager, RoleManager<Role> roleManager)
    : IPermissionService
{
    public async Task<HashSet<string>> GetPermissionsAsync(User user)
    { 
        var roleNames = await userManager.GetRolesAsync(user);

        var permissions = roleManager.Roles
            .Where(x => roleNames.Contains(x.Name!))
            .Select(x => x.Permissions!.ToString())
            .ToHashSet();

        return permissions;
    }
}
