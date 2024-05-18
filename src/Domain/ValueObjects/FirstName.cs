using Domain.Core.Errors;
using Domain.Core.Primitives;
using Domain.Core.Primitives.Result;

namespace Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 100;

    private FirstName(string value) => Value = value;

    public static implicit operator string (FirstName value) => value.Value;
    
    public string Value { get;}

    public static Result<FirstName> Create(string firstName)
    {
        if (string.IsNullOrEmpty(firstName)) return Result.Failure<FirstName>(DomainErrors.FirstName.NullOrEmpty);

        if (firstName.Length > MaxLength) return Result.Failure<FirstName>(DomainErrors.FirstName.LongerThanAllowed);

        return new FirstName(firstName);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
