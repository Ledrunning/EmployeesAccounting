using EA.Repository.Contracts;
using EA.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace EA.Repository.Repository;

public class AdministratorRepository : BaseRepository<Administrator>, IAdministratorRepository
{
    public AdministratorRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<Administrator?> GetByCredentialsAsync(string login, string pass, CancellationToken token)
    {
        return await DbContext.Set<Administrator>().AsNoTracking()
            .FirstOrDefaultAsync(i => i.Login == login && i.Password == pass, token);
    }
}