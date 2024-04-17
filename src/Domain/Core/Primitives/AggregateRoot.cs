using Domain.Core.Events;

namespace Domain.Core.Primitives;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid Id) : base(Id) { }

    protected AggregateRoot() { }

    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
}
