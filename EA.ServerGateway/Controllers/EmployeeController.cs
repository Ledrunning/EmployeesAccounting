using EA.Services.Contracts;
using EA.Services.Dto;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    [Route(nameof(GetAllEmployee))]
    public async Task<IReadOnlyList<EmployeeDto?>> GetAllEmployee(CancellationToken cancellationToken)
    {
        return await _employee.GetAllAsync(cancellationToken);
    }

    [HttpGet]
    [Route(nameof(GetEmployeeById))]
    public async Task<EmployeeDto?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        return await _employee.GetByIdAsync(id, cancellationToken);
    }

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<EmployeeDto?> Create([FromBody] EmployeeDto employee, CancellationToken cancellationToken)
    {
        return await _employee.AddAsync(employee, cancellationToken);
    }

    [HttpPost]
    [Route(nameof(Update))]
    public async Task Update(EmployeeDto employee, CancellationToken cancellationToken)
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