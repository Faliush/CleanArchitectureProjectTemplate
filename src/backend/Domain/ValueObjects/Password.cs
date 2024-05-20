using Domain.Core.Errors;
using Domain.Core.Primitives;
using Domain.Core.Primitives.Result;

namespace Domain.ValueObjects;

public sealed class Password : ValueObject
{
    private const int MinPasswordLength = 6;
    private static readonly Func<char, bool> IsLower = c => c >= 'a' && c <= 'z';
    private static readonly Func<char, bool> IsUpper = c => c >= 'A' && c <= 'Z';
    private static readonly Func<char, bool> IsDigit = c => c >= '0' && c <= '9';
    private static readonly Func<char, bool> IsNonAlphaNumeric = c => !(IsLower(c) || IsUpper(c) || IsDigit(c));

    private Password(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Password password) => password?.Value ?? string.Empty;

    public static Result<Password> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return Result.Failure<Password>(DomainErrors.Password.NullOrEmpty);

        if (password.Length < MinPasswordLength) return Result.Failure<Password>(DomainErrors.Password.TooShort);

        if(!password.Any(IsLower)) return Result.Failure<Password>(DomainErrors.Password.MissingLowercaseLetter);
        
        if(!password.Any(IsUpper)) return Result.Failure<Password>(DomainErrors.Password.MissingUppercaseLetter);
        
        if(!password.Any(IsDigit)) return Result.Failure<Password>(DomainErrors.Password.MissingDigit);
        
        if(password.Any(IsNonAlphaNumeric)) return Result.Failure<Password>(DomainErrors.Password.MissingNonAlphaNumeric);

        return new Password(password);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
