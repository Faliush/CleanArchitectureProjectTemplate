using Domain.Core.Abstractions;

namespace Domain.Core.Primitives;

public abstract class SoftDeletable : ISoftDeletable
{
    public DateTime? DeletedOnUtc {  get; }

    public bool Deleted {  get; }
}
