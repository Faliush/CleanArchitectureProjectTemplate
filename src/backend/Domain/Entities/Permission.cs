using Domain.Core.Primitives;

namespace Domain.Entities;

public sealed class Permission : Entity
{
    public string Name { get; set; } = string.Empty;

    public IList<Role> Roles { get; set; } = [];
}