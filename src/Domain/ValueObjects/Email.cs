using Domain.Core.Errors;
using Domain.Core.Primitives;
using Domain.Core.Primitives.Result;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public const int MaxLength = 256;

    private const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

    private static readonly Lazy<Regex> EmailFormatRegex =
        new(() => new Regex(EmailRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));

    private Email(string value) => Value = value; 
        
    public string Value { get; } 

    public static implicit operator string(Email email) => email.Value;

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrEmpty(email)) return Result.Failure<Email>(DomainErrors.Email.NullOrEmpty);

        if (email.Length > MaxLength) return Result.Failure<Email>(DomainErrors.Email.LongerThanAllowed);

        if (EmailFormatRegex.Value.IsMatch(email)) return Result.Failure<Email>(DomainErrors.Email.InvalidFormat);

        return new Email(email);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
