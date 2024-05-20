using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Application.Abstractions.Authentication.Attribute;

internal sealed class HasPermissionAttribute(Permission permission)
    : AuthorizeAttribute(policy: nameof(permission))
{
}
