using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork<out TContext>
    : IUnitOfWork where TContext : DbContext
{ 
    TContext DbContext { get; }
}


public interface IUnitOfWork : IDisposable
{
    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    IDbContextTransaction BeginTransaction(bool useIfExists = false);

    Task<IDbContextTransaction> BeginTransactionAsync(bool useIfExists = false);
}
