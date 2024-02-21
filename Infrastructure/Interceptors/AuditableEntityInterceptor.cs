using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Domain.Base;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interceptors;

public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private const string DefaultUserName = "Anonymous";
    private readonly DateTime DefaultDate = DateTime.UtcNow.ToUniversalTime();
    private readonly IHttpContextAccessor _contextAccessor;

    public AuditableEntityInterceptor(IHttpContextAccessor contextAccessor)
        => _contextAccessor = contextAccessor;

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
        var entries = context
            .ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditable 
                   && ( e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach(var entityEntry in entries)
        {
            if(entityEntry.State is EntityState.Added)
            {
                ((IAuditable)entityEntry.Entity).CreatedAt = DefaultDate;
                ((IAuditable)entityEntry.Entity).CreatedBy = _contextAccessor?.HttpContext?.User?.Identity?.Name ?? DefaultUserName;
            }
            else
            {
                entityEntry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                entityEntry.Property(nameof(IAuditable.CreatedBy)).IsModified = false;
            }

            ((IAuditable)entityEntry.Entity).UpdatedAt = DefaultDate;
            ((IAuditable)entityEntry.Entity).UpdatedBy = _contextAccessor?.HttpContext?.User?.Identity?.Name ?? DefaultUserName;
        }
    }

}
