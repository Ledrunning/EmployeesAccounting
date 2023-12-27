using EA.DesktopApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace EA.DesktopApp.Contracts
{
    public interface IAdminGatewayService : ICredentialService
    {
        Task<bool> Login(CancellationToken token);
        Task<bool> Login(Credentials credentials, CancellationToken token);
        Task<IReadOnlyList<AdministratorModel>> GetAllAsync(CancellationToken token);
        Task<AdministratorModel> GetByIdAsync(long id, CancellationToken token);
        Task<AdministratorModel> ChangeLoginAsync(Credentials credentials, CancellationToken token);
        Task CreateAsync(AdministratorModel admin, CancellationToken token);
        Task UpdateAsync(AdministratorModel admin, CancellationToken token);
        Task DeleteAsync(long id, CancellationToken token);
    }
}