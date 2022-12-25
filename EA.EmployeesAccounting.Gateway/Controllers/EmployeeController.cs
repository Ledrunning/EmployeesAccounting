using EA.ServerGateway.Model;
using EA.ServerGateway.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EA.ServerGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        /// <summary>
        ///     .ctor
        /// </summary>
        /// <param name="employee"></param>
        public EmployeeController(IEmployeeRepository employee)
        {
            Employee = employee;
        }

        /// <summary>
        ///     Instance of interface
        /// </summary>
        public IEmployeeRepository Employee { get; set; }

        /// <summary>
        ///     Get all persons from data base
        /// </summary>
        /// <returns></returns>
        public IQueryable<Employee> GetAllEmployee()
        {
            return Employee.GetAllEmployees();
        }


        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetEmployeeById(int id)
        {
            var item = Employee.GetEmployeeById(id);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Employee employee)
        {
            Employee.AddEmployee(employee);
            return CreatedAtRoute("GetTodo", new {id = employee.Id}, employee);
        }

        /// <summary>
        ///     Not used
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Employee employee)
        {
            //if (employee == null || employee.Id != id)
            //{
            //    return BadRequest();
            //}

            //var todo = Person.Find(id);
            //if (todo == null)
            //{
            //    return NotFound();
            //}

            //Person.UpdateEmployeeData(employee);
            //return new NoContentResult();
            return null;
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromBody] Employee employee, string id)
        {
            //if (employee == null)
            //{
            //    return BadRequest();
            //}

            //var todo = Person.Find(id);
            //if (todo == null)
            //{
            //    return NotFound();
            //}

            //employee.Key = todo.Key;

            //Person.UpdateEmployeeData(employee);
            //return new NoContentResult();
            return null;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = Employee.GetEmployeeById(id);
            if (todo == null)
            {
                return NotFound();
            }

            Employee.RemoveEmployeeById(id);
            return new NoContentResult();
        }
    }
}