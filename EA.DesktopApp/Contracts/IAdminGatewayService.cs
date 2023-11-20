using EA.DesktopApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace EA.DesktopApp.Contracts
{
    public interface IAdminGatewayService : ICredentialService
    {
        Task<bool> Login(CancellationToken token);
        Task<IReadOnlyList<AdministratorModel>> GetAllAsync(CancellationToken token);
        Task<AdministratorModel> GetByIdAsync(long id, CancellationToken token);
        Task CreateAsync(AdministratorModel admin, CancellationToken token);
        Task UpdateAsync(AdministratorModel admin, CancellationToken token);
        Task DeleteAsync(long id, CancellationToken token);
    }
}