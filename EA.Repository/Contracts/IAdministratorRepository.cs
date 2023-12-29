using EA.Repository.Entities;

namespace EA.Repository.Contracts;

public interface IAdministratorRepository : IAsyncRepository<Administrator>
{
    Task<Administrator?> GetByCredentialsAsync(string? login, CancellationToken token);
}