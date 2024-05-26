namespace Domain.Entities;

public sealed class UserRole
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
}
