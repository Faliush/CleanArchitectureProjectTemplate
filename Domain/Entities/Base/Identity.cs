namespace Domain.Entities.Base;

public abstract class Identity : IHaveId
{
    public Guid Id { get; set; }
}
