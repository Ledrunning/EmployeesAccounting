using EA.ServerGateway.Model;

namespace EA.ServerGateway.Repository
{
    /// <summary>
    ///     For IoC
    /// </summary>
    public interface IEmployeeRepository
    {
        Employee AddEmployee(Employee employee);
        Employee GetEmployeeById(int id);
        IQueryable<Employee> GetAllEmployees();
        void RemoveEmployeeById(int id);
        bool UpdateEmployeeData(Employee employee);
    }
}