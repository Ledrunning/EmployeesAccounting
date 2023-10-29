﻿using EA.Services.Dto;

namespace EA.Services.Contracts;

public interface IAdministratorService
{
    Task InitializeAdmin(CancellationToken token);
    Task AddAsync(AdministratorDto admin, CancellationToken cancellationToken);
    Task<IReadOnlyList<AdministratorDto?>> GetAllAsync(CancellationToken cancellationToken);
    Task<AdministratorDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<bool> LoginAsync(string username, string password, CancellationToken token);
    Task UpdateAsync(AdministratorDto employee, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}