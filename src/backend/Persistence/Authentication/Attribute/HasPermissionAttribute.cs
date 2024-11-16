using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Persistence.Authentication.Attribute;

public sealed class HasPermissionAttribute(params Permissions[] permissions)
    : AuthorizeAttribute(policy: string.Join(',', permissions))
{
}
