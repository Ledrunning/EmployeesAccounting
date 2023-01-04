using EA.Services.Models;

namespace EA.Services.Contracts;

public interface IEmployeeGatewayService
{
    Task<IReadOnlyList<EmployeeDto>> GetAllEmployee(CancellationToken cancellationToken);
    Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<EmployeeDto?> Create(EmployeeDto employee, CancellationToken cancellationToken);
    Task<EmployeeDto?> Update(EmployeeDto employee, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}