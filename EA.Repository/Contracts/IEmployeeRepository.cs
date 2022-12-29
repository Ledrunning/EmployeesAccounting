using EA.Repository.Entities;

namespace EA.Repository.Contracts
{
    public interface IEmployeeRepository
    {
        Employee AddEmployee(Employee employee);
        Employee GetEmployeeById(int id);
        IQueryable<Employee> GetAllEmployees();
        void RemoveEmployeeById(int id);
        bool UpdateEmployeeData(Employee employee);
    }
}