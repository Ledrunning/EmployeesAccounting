﻿using EA.Repository.Contracts;
using EA.Repository.Entities;

namespace EA.Repository.Repository;

public class AdministratorRepository : BaseRepository<Administrator>, IAdminRepository
{
    public AdministratorRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }
}