using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Application.Abstractions.Authentication.Attribute;

public sealed class HasPermissionAttribute(Permissions permission)
    : AuthorizeAttribute(policy: permission.ToString())
{
}
