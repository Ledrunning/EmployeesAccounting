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

    public virtual async Task<T?> GetByIdAsync(long id, CancellationToken token)
    {
        return await DbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, cancellationToken: token);
    }

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken token)
    {
        var queryable = await DbContext.Set<T>().AsNoTracking().AsQueryable().ToListAsync(cancellationToken: token);

        return queryable;
    }

    public async Task<T> AddAsync(T entity, CancellationToken token)
    {
        await DbContext.Set<T>().AddAsync(entity, token);
        await SaveChangesAsync(token);

        return entity;
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken token)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
        var affectedRows = await SaveChangesAsync(token);
        return affectedRows > 0;
    }

    public async Task DeleteAsync(T entity, CancellationToken token)
    {
        DbContext.Set<T>().Remove(entity);
        await SaveChangesAsync(token);
    }

    public async Task DeleteAsync(long id, CancellationToken token)
    {
        var entity = await DbContext.Set<T>().FindAsync(id);
        if (entity == null)
        {
            return;
        }

        DbContext.Set<T>().Remove(entity);
        await SaveChangesAsync(token);
    }

    protected async Task<int> SaveChangesAsync(CancellationToken token)
    {
        return await DbContext.SaveChangesAsync(token);
    }
}