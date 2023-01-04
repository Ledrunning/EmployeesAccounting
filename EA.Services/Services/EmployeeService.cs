using AutoMapper;
using EA.Repository.Contracts;
using EA.Repository.Entities;
using EA.Services.Contracts;
using EA.Services.Models;

namespace EA.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.ListAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<EmployeeDto>>(employee);
    }

    public async Task<EmployeeDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto?> AddAsync(EmployeeDto employee, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Employee>(employee);
        var addedEmployee = await _employeeRepository.AddAsync(entity, cancellationToken);
        return _mapper.Map<EmployeeDto>(addedEmployee);
    }

    public async Task UpdateAsync(EmployeeDto employee, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Employee>(employee);
        await _employeeRepository.UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await _employeeRepository.DeleteAsync(id, cancellationToken);
    }
}