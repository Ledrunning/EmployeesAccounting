using System.Linq;
using EA.WebApiServerSide.Model;

namespace EA.WebApiServerSide.Repository
{
    /// <summary>
    ///     For IoC
    /// </summary>
    public interface IEmployeeRepository
    {
        Person AddEmployee(Person person);
        Person GetEmployeeById(int id);
        IQueryable<Person> GetAllEmployees();
        void RemoveEmployeeById(int id);
        bool UpdateEmployeeData(Person person);
    }
}