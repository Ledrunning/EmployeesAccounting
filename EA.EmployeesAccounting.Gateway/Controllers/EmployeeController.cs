using EA.Repository.Model;
using EA.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EA.ServerGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : Controller
{

    private readonly IEmployeeRepository _employee;

    public EmployeeController(IEmployeeRepository employee)
    {
        _employee = employee;
    }

    /// <summary>
    ///     Get all employees from data base
    /// </summary>
    /// <returns>IQueryable<Employee></returns>
    [HttpGet]
    [Route(nameof(GetAllEmployee))]
    public IQueryable<Employee> GetAllEmployee()
    {
        return _employee.GetAllEmployees();
    }


    [HttpGet]
    [Route(nameof(GetEmployeeById))]
    public IActionResult GetEmployeeById(int id)
    {
        var item = _employee.GetEmployeeById(id);
        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }

    [HttpPost]
    [Route(nameof(Create))]
    public IActionResult Create([FromBody] Employee employee)
    {
        _employee.AddEmployee(employee);
        return CreatedAtRoute("GetTodo", new { id = employee.Id }, employee);
    }

    /// <summary>
    ///     Not used
    /// </summary>
    /// <param name="id"></param>
    /// <param name="employee"></param>
    /// <returns></returns>
    [HttpPut]
    [Route(nameof(Update))]
    public IActionResult Update(string id, [FromBody] Employee employee)
    {
        //if (employee == null || employee.Id != id)
        //{
        //    return BadRequest();
        //}

        //var todo = Employee.Find(id);
        //if (todo == null)
        //{
        //    return NotFound();
        //}

        //Employee.UpdateEmployeeData(employee);
        //return new NoContentResult();
        return null;
    }

    [HttpDelete]
    [Route(nameof(Delete))]
    public IActionResult Delete(int id)
    {
        var todo = _employee.GetEmployeeById(id);
        if (todo == null)
        {
            return NotFound();
        }

        _employee.RemoveEmployeeById(id);
        return new NoContentResult();
    }
}