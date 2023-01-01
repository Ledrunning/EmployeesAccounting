using EA.ServerGateway.Models;

namespace EA.ServerGateway;

public interface IEmployeeService
{
    Task<IReadOnlyList<EmployeeDto>> GetAllEmployeeAsync(CancellationToken cancellationToken);
    Task<EmployeeDto?> GetEmployeeByIdAsync(long id, CancellationToken cancellationToken);
    Task<EmployeeDto?> AddEmployeeAsync(EmployeeDto employee, CancellationToken cancellationToken);
    Task UpdateEmployeeAsync(EmployeeDto employee, CancellationToken cancellationToken);
    Task DeleteEmployeeAsync(long id, CancellationToken cancellationToken);
}