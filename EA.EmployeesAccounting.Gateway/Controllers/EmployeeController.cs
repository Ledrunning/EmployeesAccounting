using EA.Repository.Contracts;
using EA.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

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
    public async Task<IReadOnlyList<Employee>> GetAllEmployee(CancellationToken cancellationToken)
    {
        return await _employee.ListAsync(cancellationToken);
    }
    
    [HttpGet]
    [Route(nameof(GetEmployeeById))]
    public async Task<Employee?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        return await _employee.GetByIdAsync(id, cancellationToken);
    }

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<Employee> Create([FromBody] Employee employee, CancellationToken cancellationToken)
    {
        return await _employee.AddAsync(employee, cancellationToken);
    }

    /// <summary>
    ///     Not used
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    [HttpPost]
    [Route(nameof(Update))]
    public async Task Update(Employee employee, CancellationToken cancellationToken)
    {
         await _employee.UpdateAsync(employee, cancellationToken);
    }

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var employee = await _employee.GetByIdAsync(id, cancellationToken);
        if (employee == null)
        {
            return NotFound();
        }

        await _employee.DeleteAsync(id, cancellationToken);
        return new NoContentResult();
    }
}