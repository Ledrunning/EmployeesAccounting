using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EA.DesktopApp.Models;

namespace EA.DesktopApp.Contracts
{
    public interface IEmployeeGatewayService : ICredentialService
    {
        Task<IReadOnlyList<EmployeeModel>> GetAllEmployeeAsync(CancellationToken token);
        Task<IReadOnlyList<EmployeeModel>> GetAllWithPhotoAsync(CancellationToken token);
        Task<EmployeeModel> GetByIdAsync(long id, CancellationToken token);
        Task<string> GetNameByIdAsync(long id, CancellationToken token);
        Task CreateAsync(EmployeeModel employee, CancellationToken token);
        Task UpdateAsync(EmployeeModel employee, CancellationToken token);
        Task DeleteAsync(long id, CancellationToken token);

        Task<bool> IsServerAvailableAsync(CancellationToken token);
    }
}