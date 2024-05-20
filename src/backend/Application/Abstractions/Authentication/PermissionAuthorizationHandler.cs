using Application.Abstractions.Authentication.Claims;
using Microsoft.AspNetCore.Authorization;


namespace Application.Abstractions.Authentication;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // for checking permission using db 
        //var userId = context.User.Claims
        //    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        //if (!Guid.TryParse(userId, out Guid parsedUserId))
        //{
        //    return;
        //}

        //var scope = serviceScopeFactory.CreateScope();

        //var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        //var permissions = await permissionService.GetPermissionsAsync(parsedUserId);

        var permissions = context.User.Claims
            .Where(x => x.Type == CustomClaims.Permission)
            .Select(x => x.Value)
            .ToHashSet();

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
