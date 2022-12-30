using EA.Repository.Contracts;
using EA.Repository.Entities;

namespace EA.Repository.Repository;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(DatabaseContext employeeContext) : base(employeeContext)
    {
    }
}