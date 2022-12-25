using EA.ServerGateway.Model;

namespace EA.ServerGateway.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        //TodoContext empContext;

        /// <summary>
        ///     .ctor instantiation
        /// </summary>
        /// <param name="employeeContext"></param>
        public EmployeeRepository(EmployeeContext employeeContext)
        {
            this._employeeContext = employeeContext;
        }

        /// <summary>
        ///     Add user to database
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public Employee AddEmployee(Employee employee)
        {
            //employee.Id = Guid.NewGuid();
            _employeeContext.Person.Add(employee);
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
            return _employeeContext.Person.SingleOrDefault(c => c.Id == id);
        }

        /// <summary>
        ///     Get all data from data base
        /// </summary>
        /// <returns></returns>
        public IQueryable<Employee> GetAllEmployees()
        {
            return _employeeContext.Person.AsQueryable();
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
                var personToRemove = new Employee {Id = id};
                _employeeContext.Person.Attach(personToRemove);
                _employeeContext.Person.Remove(personToRemove);
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
}