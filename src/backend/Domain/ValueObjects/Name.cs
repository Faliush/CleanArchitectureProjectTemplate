using Domain.Core.Errors;
using Domain.Core.Primitives;
using Domain.Core.Primitives.Result;

namespace Domain.ValueObjects;

public sealed class Name : ValueObject
{
    public const int MaxLength = 100;

    private Name(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Name name) => name.Value;

    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrEmpty(name)) return Result.Failure<Name>(DomainErrors.Role.NullOrEmpty);

        if(name.Length > MaxLength) return Result.Failure<Name>(DomainErrors.Role.LongerThanAllowed);

        return new Name(name);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
