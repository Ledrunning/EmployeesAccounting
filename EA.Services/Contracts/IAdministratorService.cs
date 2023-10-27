using EA.Services.Dto;

namespace EA.Services.Contracts;

public interface IAdministratorService
{
    Task AddAsync(AdministratorDto admin, CancellationToken cancellationToken);
    Task<IReadOnlyList<AdministratorDto?>> GetAllAsync(CancellationToken cancellationToken);
    Task<AdministratorDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<AdministratorDto?> GetByCredentialsAsync(string login, string pass, CancellationToken token)
    Task UpdateAsync(AdministratorDto employee, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}