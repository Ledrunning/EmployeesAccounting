using EA.ServerGateway.Dto;

namespace EA.ServerGateway.Contracts
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<EmployeeDto?>> GetAllAsync(CancellationToken cancellationToken);
        Task<EmployeeDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<EmployeeDto?> AddAsync(EmployeeDto employee, CancellationToken cancellationToken);
        Task UpdateAsync(EmployeeDto employee, CancellationToken cancellationToken);
        Task DeleteAsync(long id, CancellationToken cancellationToken);
    }
}