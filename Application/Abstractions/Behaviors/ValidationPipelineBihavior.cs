using FluentValidation;
using MediatR;

namespace Application.Abstractions.Behaviors;

public sealed class ValidationPipelineBihavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBihavior(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var failures = _validators
                .Select(x => x.Validate(new ValidationContext<TRequest>(request)))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

        if (!failures.Any())
            return next();

        throw new ValidationException(failures);
    }
}
