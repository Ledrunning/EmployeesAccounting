using EA.DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EA.DesktopApp.Contracts
{
    public interface IEmployeeGatewayService
    {
        Task<IReadOnlyList<EmployeeModel>> GetAllEmployeeAsync(CancellationToken cancellationToken);
        Task<EmployeeModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<EmployeeModel> CreateAsync(EmployeeModel employee, CancellationToken cancellationToken);
        Task<EmployeeModel> UpdateAsync(EmployeeModel employee, CancellationToken cancellationToken);
        Task DeleteAsync(long id, CancellationToken cancellationToken);
    }
}