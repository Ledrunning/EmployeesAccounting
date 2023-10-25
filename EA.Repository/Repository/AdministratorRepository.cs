using EA.Repository.Contracts;
using EA.Repository.Entities;

namespace EA.Repository.Repository;

public class AdministratorRepository : BaseRepository<Administrator>, IAdministratorRepository
{
    public AdministratorRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }
}