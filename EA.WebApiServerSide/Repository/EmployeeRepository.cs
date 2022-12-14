using System;
using System.Linq;
using EA.WebApiServerSide.Model;

namespace EA.WebApiServerSide.Repository
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
        /// <param name="person"></param>
        /// <returns></returns>
        public Person AddEmployee(Person person)
        {
            //person.Id = Guid.NewGuid();
            _employeeContext.Person.Add(person);
            _employeeContext.SaveChanges();
            return person;
        }

        /// <summary>
        ///     Get person by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Person GetEmployeeById(int id)
        {
            return _employeeContext.Person.SingleOrDefault(c => c.Id == id);
        }

        /// <summary>
        ///     Get all data from data base
        /// </summary>
        /// <returns></returns>
        public IQueryable<Person> GetAllEmployees()
        {
            return _employeeContext.Person.AsQueryable();
        }

        /// <summary>
        ///     RemoveEmployeeById person from data base
        ///     using Id
        /// </summary>
        /// <param name="id"></param>
        public void RemoveEmployeeById(int id)
        {
            try
            {
                var personToRemove = new Person {Id = id};
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
        /// <param name="person"></param>
        /// <returns></returns>
        public bool UpdateEmployeeData(Person person)
        {
            throw new NotImplementedException();
        }
    }
}