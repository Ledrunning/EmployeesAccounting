using EA.Repository.Contracts;
using EA.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using EA.ServerGateway.Models;

namespace EA.ServerGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : Controller
{
    private readonly IEmployeeService _employee;

    public EmployeeController(IEmployeeService employee)
    {
        _employee = employee;
    }

    /// <summary>
    ///     Get all employees from data base
    /// </summary>
    /// <returns>IQueryable<Employee></returns>
    [HttpGet]
    [Route(nameof(GetAllEmployee))]
    public async Task<IReadOnlyList<EmployeeDto>> GetAllEmployee(CancellationToken cancellationToken)
    {
        return await _employee.GetAllEmployeeAsync(cancellationToken);
    }
    
    [HttpGet]
    [Route(nameof(GetEmployeeById))]
    public async Task<EmployeeDto?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        return await _employee.GetEmployeeByIdAsync(id, cancellationToken);
    }

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<EmployeeDto?> Create([FromBody] EmployeeDto employee, CancellationToken cancellationToken)
    {
        return await _employee.AddEmployeeAsync(employee, cancellationToken);
    }

    /// <summary>
    ///     Not used
    /// </summary>
    /// <param name="employee"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Route(nameof(Update))]
    public async Task Update(EmployeeDto employee, CancellationToken cancellationToken)
    {
         await _employee.UpdateEmployeeAsync(employee, cancellationToken);
    }

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var employee = await _employee.GetEmployeeByIdAsync(id, cancellationToken);
        if (employee == null)
        {
            return NotFound();
        }

        await _employee.DeleteEmployeeAsync(id, cancellationToken);
        return new NoContentResult();
    }
}