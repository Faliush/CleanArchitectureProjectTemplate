using Domain.Core.Abstractions;
using Domain.Core.Events;

namespace Domain.Core.Primitives;

public abstract class Entity : IHaveId, IEquatable<Entity>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    protected Entity(Guid id)
        : this() => Id = id;

    protected Entity() {  }

    public Guid Id { get; init; }


    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public static bool operator == (Entity first, Entity second)
        => first is not null && second is not null && first.Equals(second);

    public static bool operator != (Entity first, Entity second) 
        => !(first == second);

    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity other)
        {
            return false;
        }

        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode() * 41;
}

