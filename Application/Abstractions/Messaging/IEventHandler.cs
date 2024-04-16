using MediatR;

namespace Application.Abstractions.Messaging;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : INotification
{
}
