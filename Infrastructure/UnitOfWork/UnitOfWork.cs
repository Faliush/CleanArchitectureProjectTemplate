using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork;

public sealed class UnitOfWork<TContext> : IUnitOfWork<TContext>
    where TContext : DbContext
{
    private bool _disposed;

    public UnitOfWork(TContext context)
    {
        DbContext = context ?? throw new ArgumentNullException(nameof(context));
        LastSaveChangeResult = new SaveChangesResult();
    }

    public TContext DbContext { get; }
    public SaveChangesResult LastSaveChangeResult { get; }

    public int SaveChanges()
    {
        try
        {
            return DbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            LastSaveChangeResult.Exception = ex;
            return 0;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await DbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            LastSaveChangeResult.Exception = ex;
            return 0;
        }
    }

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
                //_repositories?.Clear();
                DbContext.Dispose();
            }
        }

        _disposed = true;
    }
}
