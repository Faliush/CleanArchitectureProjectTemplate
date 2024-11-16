using Microsoft.AspNetCore.Authorization;

namespace Persistence.Authentication;

public class PermissionRequirement(string[] permissions) 
    : IAuthorizationRequirement
{
    public IEnumerable<string> AllowedPermissions { get; } = permissions;
}
