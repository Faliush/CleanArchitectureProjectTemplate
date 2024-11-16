using Microsoft.AspNetCore.Authorization;

namespace Persistence.Authentication;

public class PermissionRequirement(string permission) 
    : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
