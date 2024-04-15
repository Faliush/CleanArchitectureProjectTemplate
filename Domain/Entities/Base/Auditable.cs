namespace Domain.Entities.Base;

public abstract class Auditable : Identity, IAuditable
{
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}