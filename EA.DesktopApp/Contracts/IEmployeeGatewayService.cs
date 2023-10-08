using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EA.DesktopApp.Models;

namespace EA.DesktopApp.Contracts
{
    public interface IEmployeeGatewayService
    {
        Task<IReadOnlyList<EmployeeModel>> GetAllEmployeeAsync(CancellationToken cancellationToken);
        Task<EmployeeModel> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<string> GetNameByIdAsync(long id, CancellationToken cancellationToken);
        Task CreateAsync(EmployeeModel employee, CancellationToken cancellationToken);
        Task UpdateAsync(EmployeeModel employee, CancellationToken cancellationToken);
        Task DeleteAsync(long id, CancellationToken cancellationToken);
    }
}