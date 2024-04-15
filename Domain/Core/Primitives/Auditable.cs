using Domain.Core.Abstractions;

namespace Domain.Core.Primitives;

public abstract class Auditable : IAuditable
{
    public DateTime CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }
}