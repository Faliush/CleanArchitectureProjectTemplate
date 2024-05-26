using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations.Base;

namespace Infrastructure.Repositories.Implementations;

internal sealed class PermissionRepository(ApplicationDbContext dbContext) 
    : RepositoryBase<Permission>(dbContext), IPermissionRepository
{
}
