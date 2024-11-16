
using Domain.Enums;
using Microsoft.AspNetCore.Builder;

namespace Persistence.Authentication.Attribute;

public static class RequirePermissionExtension
{
    public static TBuilder RequirePermission<TBuilder>(this TBuilder builder, params Permissions[] permissions)
        where TBuilder : IEndpointConventionBuilder
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (permissions.Length == 0)
        {
            throw new ArgumentNullException(nameof(permissions));
        }

        return builder.RequireAuthorization(string.Join(',', permissions));
    }
}