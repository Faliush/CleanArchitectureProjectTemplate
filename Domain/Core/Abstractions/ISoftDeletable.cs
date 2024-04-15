namespace Domain.Core.Abstractions;

public interface ISoftDeletable
{
    DateTime? DeletedOnUtc { get; }

    bool Deleted { get; }
}
