using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class Role : IdentityRole<Guid>
{
    public List<Permissions>? Permissions { get;  set; }
}
