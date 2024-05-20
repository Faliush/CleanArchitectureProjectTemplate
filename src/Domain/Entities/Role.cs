using Domain.Core.Primitives;

namespace Domain.Entities;

public sealed class Role(int id, string name) : Enumeration<Role>(id, name)
{
    public static readonly Role Administrator = new(1, nameof(Administrator));
    public static readonly Role Registered = new(2, nameof(Registered));

    public ICollection<Permission>? Permissions { get; set; }
    public ICollection<User>? Users { get; set; }
}
