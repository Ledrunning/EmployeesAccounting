using System.Linq;
using EA.WebApiServerSide.Model;
using EA.WebApiServerSide.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EA.WebApiServerSide.Controllers
{
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
        public IQueryable<Person> GetAllEmployee()
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
        public IActionResult Create([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest();
            }

            Employee.AddEmployee(person);
            return CreatedAtRoute("GetTodo", new {id = person.Id}, person);
        }

        /// <summary>
        ///     Not used
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Person person)
        {
            //if (person == null || person.Id != id)
            //{
            //    return BadRequest();
            //}

            //var todo = Person.Find(id);
            //if (todo == null)
            //{
            //    return NotFound();
            //}

            //Person.UpdateEmployeeData(person);
            //return new NoContentResult();
            return null;
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromBody] Person item, string id)
        {
            //if (item == null)
            //{
            //    return BadRequest();
            //}

            //var todo = Person.Find(id);
            //if (todo == null)
            //{
            //    return NotFound();
            //}

            //item.Key = todo.Key;

            //Person.UpdateEmployeeData(item);
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