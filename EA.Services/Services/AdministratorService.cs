using AutoMapper;
using EA.Common.Exceptions;
using EA.Repository.Contracts;
using EA.Repository.Entities;
using EA.Services.Contracts;
using EA.Services.Dto;
using Microsoft.Extensions.Logging;

namespace EA.Services.Services;

public class AdministratorService : IAdministratorService
{
    private readonly IAdministratorRepository _administratorRepository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IMapper _mapper;

    public AdministratorService(IMapper mapper,
        ILogger<EmployeeService> logger,
        IAdministratorRepository administratorRepository)
    {
        _mapper = mapper;
        _logger = logger;
        _administratorRepository = administratorRepository;
    }

    public async Task AddAsync(AdministratorDto employee, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Administrator>(employee);
            await _administratorRepository.AddAsync(entity, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to add an admin into the database!: {e}", e);
            throw new EmployeeAccountingException("Failed to add an admin into the database!", e);
        }
    }

    public async Task<IReadOnlyList<AdministratorDto?>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var administrators = await _administratorRepository.ListAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<AdministratorDto>>(administrators);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when trying to get all administrators: {e}", e);
            throw new EmployeeAccountingException("Error when trying to get all administrators", e);
        }
    }

    public async Task<AdministratorDto?> GetByCredentialsAsync(string login, string pass, CancellationToken token)
    {
        try
        {
            var administrator = await _administratorRepository.GetByCredentialsAsync(login, pass, token);
            return _mapper.Map<AdministratorDto>(administrator);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when trying to get an administrator by credentials: {e}", e);
            throw new EmployeeAccountingException("Error when trying to get an administrator by credentials", e);
        }
    }

    public async Task<AdministratorDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            var administrator = await _administratorRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<AdministratorDto>(administrator);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when trying to get an administrator by Id: {e}", e);
            throw new EmployeeAccountingException("Error when trying to get an administrator by Id", e);
        }
    }

    public async Task UpdateAsync(AdministratorDto employee, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Administrator>(employee);
            await _administratorRepository.UpdateAsync(entity, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to update an administrator in database!: {e}", e);
            throw new EmployeeAccountingException("Failed to update an administrator in database!", e);
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            await _administratorRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete an administrator in the database!: {e}", e);
            throw new EmployeeAccountingException("Failed to delete an administrator in database!", e);
        }
    }
}