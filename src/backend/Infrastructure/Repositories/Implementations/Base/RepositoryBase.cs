using Domain.Core.Primitives.Pagination;
using Infrastructure.Repositories.Contracts.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Implementations.Base;

internal abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    protected RepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableQuerySplitting = false,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!disableQuerySplitting)
            query = query.AsSplitQuery();

        if (disableTracking)
            query = query.AsNoTracking();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (ignoreAutoInclude)
            query = query.IgnoreAutoIncludes();

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
            query = include(query);

        return orderBy is not null
            ? orderBy(query)
            : query;
    }

    public IPagedList<TEntity> GetPagedList(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableQuerySplitting = false,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false
        )
    {
        IQueryable<TEntity> query = _dbSet;

        if (!disableQuerySplitting)
            query = query.AsSplitQuery();

        if (disableTracking)
            query = query.AsNoTracking();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (ignoreAutoInclude)
            query = query.IgnoreAutoIncludes();

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
            query = include(query);

        return orderBy is not null
            ? orderBy(query).ToPagedList(pageIndex, pageSize)
            : query.ToPagedList(pageIndex, pageSize);
    }

    public async Task<IList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableQuerySplitting = false,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!disableQuerySplitting)
            query = query.AsSplitQuery();

        if (disableTracking)
            query = query.AsNoTracking();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (ignoreAutoInclude)
            query = query.IgnoreAutoIncludes();

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
            query = include(query);

        return orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);
    }

    public async Task<IPagedList<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableQuerySplitting = false,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!disableQuerySplitting)
            query = query.AsSplitQuery();

        if (disableTracking)
            query = query.AsNoTracking();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (ignoreAutoInclude)
            query = query.IgnoreAutoIncludes();

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
            query = include(query);

        return orderBy is not null
            ? await orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken)
            : await query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
    }

    public TEntity? GetFirstOrDefault(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableQuerySplitting = false,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!disableQuerySplitting)
            query = query.AsSplitQuery();

        if (disableTracking)
            query = query.AsNoTracking();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (ignoreAutoInclude)
            query = query.IgnoreAutoIncludes();

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
            query = include(query);

        return orderBy is not null
            ? orderBy(query).FirstOrDefault()
            : query.FirstOrDefault();
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableQuerySplitting = false,
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        bool ignoreAutoInclude = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!disableQuerySplitting)
            query = query.AsSplitQuery();

        if (disableTracking)
            query = query.AsNoTracking();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (ignoreAutoInclude)
            query = query.IgnoreAutoIncludes();

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
            query = include(query);

        return orderBy is not null
            ? await orderBy(query).FirstOrDefaultAsync(cancellationToken)
            : await query.FirstOrDefaultAsync(cancellationToken);
    }

    public ValueTask<EntityEntry<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        _dbSet.AddAsync(entity, cancellationToken);

    public void Update(TEntity entity) =>
        _dbSet.Update(entity);

    public async Task<int> ExecuteUpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> property,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(predicate)
            .ExecuteUpdateAsync(property, cancellationToken);
    }
    public void Delete(object id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
            Delete(entity);
    }

    public void Delete(TEntity entity) =>
        _dbSet.Remove(entity);

    public async Task<int> ExecuteDeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(predicate)
            .ExecuteDeleteAsync(cancellationToken);
    }
}

