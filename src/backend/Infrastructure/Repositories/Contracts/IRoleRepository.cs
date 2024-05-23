using Domain.Entities;
using Infrastructure.Repositories.Contracts.Base;

namespace Infrastructure.Repositories.Contracts;

public interface IRoleRepository : IRepositoryBase<Role>
{
    Task<bool> Exists(int id, CancellationToken cancellationToken);
}
