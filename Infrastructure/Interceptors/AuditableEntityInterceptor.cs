using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Domain.Base;

namespace Infrastructure.Interceptors;

public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private const string DefaultUserName = "Anonymous";
    private readonly DateTime DefaultDate = DateTime.UtcNow.ToUniversalTime();

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if(eventData.Context is not null)
            DbSaveChanges(eventData.Context!);

        return base.SavedChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if(eventData.Context is not null)
            DbSaveChanges(eventData.Context!);

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void DbSaveChanges(DbContext context)
    {
        var addedEntries = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

        foreach (var entry in addedEntries)
        {
            if (entry.Entity is not IAuditable)
                continue;

            var createdAt = entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue;
            var createdBy = entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue;
            var updatedAt = entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue;
            var updatedBy = entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue;
            var userName = createdBy is null
                ? DefaultUserName
                : entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue;

            entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue = userName;
            entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = userName;

            if (createdAt != null)
            {
                if (DateTime.Parse(createdAt.ToString()).Year > 1970)
                    entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = ((DateTime)createdAt).ToUniversalTime();

                else
                    entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = DefaultDate;
            }
            else
            {
                entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = DefaultDate;
            }

            if (updatedAt != null)
            {
                if (DateTime.Parse(updatedAt.ToString()).Year > 1970)
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = ((DateTime)updatedAt).ToUniversalTime();

                else
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = DefaultDate;
            }
            else
            {
                entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = DefaultDate;
            }

            LastSaveChangesResult.AddMessage($"ChangeTracker has new entities: {entry.Entity.GetType()}");
        }

        var modifiedEntries = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

        foreach (var entry in modifiedEntries)
        {
            if (entry.Entity is not IAuditable)
                continue;

            var userName = entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue == null
                ? DefaultUserName
                : entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue;
            var updatedAt = entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = DefaultDate;
            var updatedBy = entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = userName;

            LastSaveChangesResult.AddMessage($"ChangeTracker has modified entities: {entry.Entity.GetType()}");
        }
    }

}
