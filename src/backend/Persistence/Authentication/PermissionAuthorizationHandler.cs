using Microsoft.AspNetCore.Authorization;
using Persistence.Authentication.Claims;

namespace Persistence.Authentication;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var permissions = context.User.Claims
            .Where(x => x.Type == CustomClaims.Permission)
            .Select(x => x.Value)
            .ToHashSet();

        if (requirement.AllowedPermissions.All(permissions.Contains))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
