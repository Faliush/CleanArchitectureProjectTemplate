namespace Application.Abstractions.OperationResult.Base;

public abstract class OperationResultBase
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


