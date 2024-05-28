using Domain.Enums;

namespace Application.Roles.Queries.GetAll;

public sealed class RoleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Permissions> Permissions { get; set; } = [];
}
