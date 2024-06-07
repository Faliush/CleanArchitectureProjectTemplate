using Domain.Core.Primitives.Result;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface ICommand<TResponse>
    :IRequest<Result<TResponse>> where TResponse : class
{
}

public interface ICommand
    : IRequest<Result> 
{
}
