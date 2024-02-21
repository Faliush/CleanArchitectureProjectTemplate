using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork<out TContext>
    : IUnitOfWork where TContext : DbContext
{ 
    TContext DbContext { get; }
}


public interface IUnitOfWork : IDisposable
{
    int SaveChanges();

    Task<int> SaveChangesAsync();

    SaveChangesResult LastSaveChangeResult { get; }
}
