using Microsoft.AspNetCore.Authorization;

namespace Application.Abstractions.Authentication;

public class PermissionRequirement(string permission) 
    : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
