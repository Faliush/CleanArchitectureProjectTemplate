using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Role
{
    public static readonly Role Administrator = new(1, Name.Create(nameof(Administrator)).Value);
    public static readonly Role Registered = new(2, Name.Create(nameof(Registered)).Value);

    private Role(int id, Name name)
    {
        Id = id;
        Name = name;
    }

    public Role(Name name)
        => Name = name;

    private Role() { }  

    public int Id { get; private set; }
    public Name Name { get; private set; } 

    public ICollection<Permission>? Permissions { get; set; }
    public ICollection<User>? Users { get; set; }
}
