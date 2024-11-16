using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class RoleRepository(DbContext dbContext) : RepositoryBase<Role>(dbContext), IRoleRepository;