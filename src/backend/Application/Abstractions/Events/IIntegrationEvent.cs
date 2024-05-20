using MediatR;

namespace Application.Abstractions.Events;

public interface IIntegrationEvent : INotification
{
    Guid Id { get; init; }
}
