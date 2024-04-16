namespace Domain.Core.Primitives.Result;

public class Result
{
    protected Result(bool isOk, Error error)
    {
        if (isOk && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isOk && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsOk = isOk;
        Error = error;
    }

    public bool IsOk { get; }

    public bool IsFailure => !IsOk;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TResult> Success<TResult>(TResult result) => new(result, true, Error.None);

    public static Result<TResult> Create<TResult>(TResult result, Error error) 
        where TResult : class 
        => result is null ? Failure<TResult>(error) : Success(result);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TResult> Failure<TResult>(Error error) => new(default!, false, error);
}

public class Result<TResult> : Result 
{
    private readonly TResult _result;

    protected internal Result(TResult result, bool isSuccess, Error error)
        : base(isSuccess, error)
        => _result = result;

    public static implicit operator Result<TResult>(TResult value) => Success(value);

    public TResult Value => IsOk
        ? _result
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
}


