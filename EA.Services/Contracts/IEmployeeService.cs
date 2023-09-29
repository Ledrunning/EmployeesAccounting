using EA.Services.Dto;

namespace EA.Services.Contracts
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<EmployeeDto?>> GetAllAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<EmployeeDto?>> GetAllWithPhotoAsync(CancellationToken cancellationToken);
        Task<EmployeeDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<string?> GetNameByIdAsync(long id, CancellationToken cancellationToken);

        Task AddAsync(EmployeeDto employee, CancellationToken cancellationToken);
        Task UpdateAsync(EmployeeDto employee, CancellationToken cancellationToken);
        Task DeleteAsync(long id, CancellationToken cancellationToken);
    }
}