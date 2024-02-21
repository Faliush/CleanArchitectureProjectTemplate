using MediatR;

namespace Application.Abstractions.Messaging;

public interface ICachedQuery
{
    string Key { get; }
    TimeSpan? Experation {  get; }
}

public interface ICachedQuery<TResponse> : IRequest<TResponse>, ICachedQuery;
