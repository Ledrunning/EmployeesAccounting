using EA.Repository.Contracts;
using EA.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace EA.Repository.Repository;

public class BaseRepository<T> : IAsyncRepository<T>
    where T : BaseEntity, new()
{
    protected readonly IUnitOfWork DbContext;

    public BaseRepository(IUnitOfWork dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await DbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken)
    {
        var queryable = await DbContext.Set<T>().AsNoTracking().AsQueryable().ToListAsync(cancellationToken: cancellationToken);

        return queryable;
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await DbContext.Set<T>().AddAsync(entity, cancellationToken);
        await SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        DbContext.Set<T>().Remove(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Set<T>().FindAsync(id);
        if (entity == null)
        {
            return;
        }

        DbContext.Set<T>().Remove(entity);
        await SaveChangesAsync();
    }

    protected Task SaveChangesAsync()
    {
        return DbContext.SaveChangesAsync();
    }
}