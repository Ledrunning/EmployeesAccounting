using EA.Repository.Contracts;
using EA.Repository.Entities;

namespace EA.Repository.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DatabaseContext? _employeeContext;
    //TodoContext empContext;

    /// <summary>
    ///     .ctor instantiation
    /// </summary>
    /// <param name="employeeContext"></param>
    public EmployeeRepository(DatabaseContext? employeeContext)
    {
        _employeeContext = employeeContext;
    }

    /// <summary>
    ///     Add user to database
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    public Employee AddEmployee(Employee employee)
    {
        _employeeContext?.Employee.Add(employee);
        _employeeContext.SaveChanges();
        return employee;
    }

    /// <summary>
    ///     Get employee by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Employee GetEmployeeById(int id)
    {
        return _employeeContext.Employee.SingleOrDefault(c => c.Id == id);
    }

    /// <summary>
    ///     Get all data from data base
    /// </summary>
    /// <returns></returns>
    public IQueryable<Employee> GetAllEmployees()
    {
        return _employeeContext.Employee.AsQueryable();
    }

    /// <summary>
    ///     RemoveEmployeeById employee from data base
    ///     using Id
    /// </summary>
    /// <param name="id"></param>
    public void RemoveEmployeeById(int id)
    {
        try
        {
            var personToRemove = new Employee { Id = id };
            _employeeContext.Employee.Attach(personToRemove);
            _employeeContext.Employee.Remove(personToRemove);
            _employeeContext.SaveChanges();
        }
        catch (Exception err)
        {
            throw new Exception(string.Format("Сотрудника не существует или запись удалена!" + err.Message));
        }
    }

    /// <summary>
    ///     Not used methods
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    public bool UpdateEmployeeData(Employee employee)
    {
        throw new NotImplementedException();
    }
}