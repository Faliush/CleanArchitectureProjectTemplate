using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Persistence.Authentication.Attribute;

public sealed class HasPermissionAttribute(Permissions permission)
    : AuthorizeAttribute(policy: permission.ToString())
{
}
