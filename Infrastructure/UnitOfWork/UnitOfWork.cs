using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork;

public sealed class UnitOfWork<TContext>(TContext context) : IUnitOfWork<TContext>
    where TContext : DbContext
{
    private bool _disposed;

    public TContext DbContext { get; } = context ?? throw new ArgumentNullException(nameof(context));

    public int SaveChanges()
        => DbContext.SaveChanges();

    public async Task<int> SaveChangesAsync()
        => await DbContext.SaveChangesAsync();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
        }

        _disposed = true;
    }
}
