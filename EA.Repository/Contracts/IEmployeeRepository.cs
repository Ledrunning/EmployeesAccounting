using EA.Repository.Entities;

namespace EA.Repository.Contracts;

public interface IEmployeeRepository : IAsyncRepository<Employee>
{
}