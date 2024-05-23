using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementations;

internal sealed class RoleRepository(ApplicationDbContext dbContext) : RepositoryBase<Role>(dbContext), IRoleRepository
{
    public Task<bool> Exists(int id, CancellationToken cancellationToken)
        => _dbSet.AnyAsync(x => x.Id == id, cancellationToken);
}
