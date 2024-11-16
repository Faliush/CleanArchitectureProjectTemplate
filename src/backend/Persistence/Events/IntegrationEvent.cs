using Application.Abstractions.Events;

namespace Persistence.Events;

public abstract record IntegrationEvent(Guid Id) : IIntegrationEvent;
