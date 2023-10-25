using EA.Services.Dto;

namespace EA.Services.Contracts;

public interface IAdministratorService
{
    Task AddAsync(AdministratorDto employee, CancellationToken cancellationToken);
    Task<IReadOnlyList<AdministratorDto?>> GetAllAsync(CancellationToken cancellationToken);
    Task<AdministratorDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task UpdateAsync(AdministratorDto employee, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}