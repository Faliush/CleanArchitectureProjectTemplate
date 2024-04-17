
namespace Domain.Core.Primitives.Result;

public sealed class Error : ValueObject
{
    public Error(string title, string message)
        => (Title, Message) = (title, message);

    public string Title { get; }
    public string Message { get; }

    public static implicit operator string(Error error) => error?.Title ?? string.Empty;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Title;
        yield return Message;
    }

    internal static Error None => new(string.Empty, string.Empty);
}

