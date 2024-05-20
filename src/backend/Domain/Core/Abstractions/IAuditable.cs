
namespace Domain.Core.Abstractions;

public interface IAuditable
{
    DateTime CreatedOnUtc { get; }
    DateTime? ModifiedOnUtc { get; }
}
