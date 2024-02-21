using Application.Abstractions.OperationResult.Base;

namespace Application.Abstractions.OperationResult;

[Serializable]
public class OperationResult<TResult> : OperationResultBase
{
    public TResult? Result { get; set; }

    public OperationResult<TResult> AddError(Error error)
        => new() { Error = error };

    public bool isOk => Error is null && Result is not null;
}
