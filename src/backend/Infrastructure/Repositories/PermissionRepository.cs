using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class PermissionRepository(DbContext dbContext)
    : RepositoryBase<Permission>(dbContext), IPermissionRepository;