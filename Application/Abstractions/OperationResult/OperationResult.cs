namespace Application.Abstractions.OperationResult;

[Serializable]
public abstract class OperationResult
{
    public Error? Error { get; set; }

    public static OperationResult<TResult> CreateResult<TResult>(TResult result, Error? error = null)
    {
        var operation = new OperationResult<TResult>
        {
            Result = result,
            Error = error
        };
        return operation;
    }

    public static OperationResult<TResult> CreateResult<TResult>() => CreateResult(default(TResult)!);
}

[Serializable]
public class OperationResult<TResult> : OperationResult
{
    public TResult? Result { get; set; }

    public OperationResult<TResult> AddError(Error error)
        => new() { Error = error };

    public bool isOk => Error is null && Result is not null;
}
