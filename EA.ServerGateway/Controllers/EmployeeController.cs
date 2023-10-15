using EA.Services.Contracts;
using EA.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EA.ServerGateway.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : Controller
{
    private readonly IEmployeeService _employee;

    public EmployeeController(IEmployeeService employee)
    {
        _employee = employee;
    }

    [HttpPost]
    [Route(nameof(Create))]
    public async Task Create([FromBody] EmployeeDto employee, CancellationToken cancellationToken)
    {
        await _employee.AddAsync(employee, cancellationToken);
    }

    [HttpGet]
    [Route(nameof(GetAllEmployee))]
    public async Task<IReadOnlyList<EmployeeDto?>> GetAllEmployee(CancellationToken cancellationToken)
    {
        return await _employee.GetAllAsync(cancellationToken);
    }

    [HttpGet]
    [Route(nameof(GetAllWithPhoto))]
    public async Task<IReadOnlyList<EmployeeDto?>> GetAllWithPhoto(CancellationToken cancellationToken)
    {
        return await _employee.GetAllWithPhotoAsync(cancellationToken);
    }

    [HttpGet]
    [Route(nameof(GetEmployeeById))]
    public async Task<EmployeeDto?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        return await _employee.GetByIdAsync(id, cancellationToken);
    }

    [HttpGet]
    [Route(nameof(GetEmployeeNameById))]
    public async Task<string?> GetEmployeeNameById(int id, CancellationToken cancellationToken)
    {
        return await _employee.GetNameByIdAsync(id, cancellationToken);
    }

    [HttpPut]
    [Route(nameof(Update))]
    public async Task Update(EmployeeDto employee, CancellationToken cancellationToken)
    {
        await _employee.UpdateAsync(employee, cancellationToken);
    }

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await _employee.DeleteAsync(id, cancellationToken);
    }
}