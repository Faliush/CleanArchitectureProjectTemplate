using Domain.Core.Primitives;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Role : Entity
{
    public static readonly Role Administrator = new( 
        Name.Create(nameof(Administrator)).Value, 
        [.. Enum.GetValues<Permissions>()]);

    public static readonly Role Registered = new(
        Name.Create(nameof(Registered)).Value, 
        [Enums.Permissions.User]);

    private Role(Name name, List<Permissions> permissions)
        : base(Guid.NewGuid())
    {
        Name = name;
        Permissions = permissions;
    }

    private Role() { }  

    public static Role Create(Name name, List<Permissions> permissions)
    {
        var role = new Role(name, permissions);

        return role;
    }

    public Name Name { get; private set; } 

    public ICollection<Permissions>? Permissions { get; private set; }
    public ICollection<User>? Users { get; set; }
}
