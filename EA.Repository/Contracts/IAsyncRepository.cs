using EA.Repository.Entities;

namespace EA.Repository.Contracts;

public interface IAsyncRepository<T> where T : BaseEntity, new()
{
    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}