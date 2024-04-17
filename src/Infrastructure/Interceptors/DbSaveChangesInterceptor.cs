using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Domain.Core.Abstractions;
using Domain.Core.Events;
using Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MediatR;

namespace Infrastructure.Interceptors;

public sealed class DbSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly DateTime DefaultDate = DateTime.UtcNow.ToUniversalTime();
    private readonly IPublisher _publisher;

    public DbSaveChangesInterceptor(IPublisher publisher)
        => _publisher = publisher;

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if(eventData.Context is not null)
        {
            UpdateAuditableEntities(DefaultDate, eventData.Context!);

            UpdateSoftDeletableEntities(DefaultDate, eventData.Context!);

            PublishDomainEvents(eventData.Context!, default).GetAwaiter().GetResult();
        }
            
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if(eventData.Context is not null)
        {
            UpdateAuditableEntities(DefaultDate, eventData.Context!);

            UpdateSoftDeletableEntities(DefaultDate, eventData.Context!);

            await PublishDomainEvents(eventData.Context!, cancellationToken);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DateTime utcNow, DbContext context)
    {
        foreach (EntityEntry<IAuditable> entityEntry in context.ChangeTracker.Entries<IAuditable>())
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(nameof(IAuditable.CreatedOnUtc)).CurrentValue = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(nameof(IAuditable.ModifiedOnUtc)).CurrentValue = utcNow;
            }
        }
    }

    /// <summary>
    /// Updates the entities implementing <see cref="ISoftDeletable"/> interface.
    /// </summary>
    /// <param name="utcNow">The current date and time in UTC format.</param>
    private void UpdateSoftDeletableEntities(DateTime utcNow, DbContext context)
    {
        foreach (EntityEntry<ISoftDeletable> entityEntry in context.ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entityEntry.State != EntityState.Deleted)
            {
                continue;
            }

            entityEntry.Property(nameof(ISoftDeletable.DeletedOnUtc)).CurrentValue = utcNow;

            entityEntry.Property(nameof(ISoftDeletable.Deleted)).CurrentValue = true;

            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
        }
    }

    /// <summary>
    /// Updates the specified entity entry's referenced entries in the deleted state to the modified state.
    /// This method is recursive.
    /// </summary>
    /// <param name="entityEntry">The entity entry.</param>
    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        foreach (ReferenceEntry referenceEntry in entityEntry.References.Where(r => r.TargetEntry.State == EntityState.Deleted))
        {
            referenceEntry.TargetEntry.State = EntityState.Unchanged;

            UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
        }
    }

    private async Task PublishDomainEvents(DbContext context, CancellationToken cancellationToken)
    {
        List<EntityEntry<AggregateRoot>> aggregateRoots = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entityEntry => entityEntry.Entity.DomainEvents.Any())
            .ToList();

        List<IDomainEvent> domainEvents = aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();

        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        IEnumerable<Task> tasks = domainEvents.Select(domainEvent => _publisher.Publish(domainEvent, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
