using GenericSpesifectionRepositoryApp.Domain.Common;
using GenericSpesifectionRepositoryApp.Domain.Specifications;

namespace GenericSpesifectionRepositoryApp.Domain.Repository;

public interface IRepository<T, TKey> where T : IEntity<TKey> where TKey : struct
{
    Task<T> GetByIdAsync(TKey id, bool tracking = true, CancellationToken cancellationToken = default);
    Task<T> GetBySpecAsync(ISpecification<T> spec, bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<T>> ListAsync(bool tracking = true, CancellationToken cancellationToken = default);

    Task<List<T>> ListAsync(ISpecification<T> spec, bool tracking = true,
        CancellationToken cancellationToken = default);

    Task<(List<T> Data, int Total)> ListWithPaginationAsync(ISpecification<T> spec, int pageIndex, int pageSize,
        bool tracking = true, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<List<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, bool permanent = false, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<T> entities, bool permanent = false,
        CancellationToken cancellationToken = default);
}