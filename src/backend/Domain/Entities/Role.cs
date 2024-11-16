using Domain.Core.Primitives;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class Role : Entity
{
    public string Name { get; set; } = string.Empty;
    
    public bool IsDeletable { get; set; }
    
    public bool IsDefault { get; set; }

    public IList<User>? Users { get; set; } = [];
    public IList<Permission> Permissions { get; set; } = [];
}
