using Domain.Core.Primitives.Result;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    where TResponse : class
{
}
