using GenericSpesifectionRepositoryApp.Domain.Common;
using GenericSpesifectionRepositoryApp.Domain.Repository;
using GenericSpesifectionRepositoryApp.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace GenericSpesifectionRepositoryApp.Infrastructure.Repository;

public class Repository<T, TKey> : IRepository<T, TKey>
    where T : class, IEntity<TKey>
    where TKey : struct
{
    private readonly DbContext _dbContext;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> GetByIdAsync(TKey id, bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = GetQueryable(tracking);
        return await query.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public async Task<T> GetBySpecAsync(ISpecification<T> spec, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(spec, tracking);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> ListAsync(bool tracking = true, CancellationToken cancellationToken = default)
    {
        var query = GetQueryable(tracking);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<T>> ListAsync(ISpecification<T> spec, bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(spec, tracking);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<(List<T> Data, int Total)> ListWithPaginationAsync(
        ISpecification<T> spec,
        int pageIndex,
        int pageSize,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(spec, tracking);

        if (!spec.IsPagingEnabled)
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

        var total = await query.CountAsync(cancellationToken);
        var data = await query.ToListAsync(cancellationToken);


        return (data, total);
    }

    public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(spec, false);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(spec, false);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedDate = DateTime.UtcNow;
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<List<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        var entityList = entities.ToList();
        foreach (var entity in entityList) entity.CreatedDate = DateTime.UtcNow;

        await _dbContext.Set<T>().AddRangeAsync(entityList, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entityList;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, bool permanent = false, CancellationToken cancellationToken = default)
    {
        if (permanent)
        {
            _dbContext.Set<T>().Remove(entity);
        }
        else
        {
            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities, bool permanent = false,
        CancellationToken cancellationToken = default)
    {
        if (permanent)
            _dbContext.Set<T>().RemoveRange(entities);
        else
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.UpdatedDate = DateTime.UtcNow;
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<T> GetQueryable(bool tracking = true)
    {
        return tracking ? _dbContext.Set<T>().AsQueryable() : _dbContext.Set<T>().AsNoTracking();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec, bool tracking = true)
    {
        var query = GetQueryable(tracking);
        return SpecificationEvaluator<T>.GetQuery(query, spec);
    }
}