using Domain.Core.Errors;
using Domain.Core.Primitives;
using Domain.Core.Primitives.Result;

namespace Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 100;

    private LastName(string value) => Value = value;

    public static implicit operator string(LastName lastName) => lastName.Value;

    public string Value { get; }

    public static Result<LastName> Create(string lastName)
    {
        if (string.IsNullOrEmpty(lastName)) return Result.Failure<LastName>(DomainErrors.LastName.NullOrEmpty);

        if (lastName.Length > MaxLength) return Result.Failure<LastName>(DomainErrors.LastName.LongerThanAllowed);

        return new LastName(lastName);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
