using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using AutoMapper;
using EA.Common.Exceptions;
using EA.Repository.Contracts;
using EA.Repository.Entities;
using EA.Services.Configuration;
using EA.Services.Contracts;
using EA.Services.Dto;
using EA.Services.Extension;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EA.Services.Services;

public class AdministratorService : IAdministratorService
{
    private readonly IAdministratorRepository _administratorRepository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IMapper _mapper;
    private readonly ServiceKeysConfig? _serviceKeys;

    public AdministratorService(IMapper mapper,
        ILogger<EmployeeService> logger,
        IAdministratorRepository administratorRepository,
        ConfigurationService config)
    {
        _mapper = mapper;
        _logger = logger;
        _administratorRepository = administratorRepository;
        _serviceKeys = config.LoadConfiguration();
    }

    //TODO: Smell!
    public async Task AddAsync(AdministratorDto admin, CancellationToken cancellationToken)
    {
        try
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(admin.Password);
            admin.Password = hashedPassword;

            var entity = _mapper.Map<Administrator>(admin);
            await _administratorRepository.AddAsync(entity, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to add an admin into the database!: {e}", e);
            throw new EmployeeAccountingException("Failed to add an admin into the database!", e);
        }
    }

    public async Task ChangeLoginAsync(Credentials credentials, CancellationToken token)
    {
        try
        {
            var administrator = await _administratorRepository.GetByCredentialsAsync(credentials.OldPassword, token);
            var isLogged = BCrypt.Net.BCrypt.Verify(credentials.OldPassword, administrator?.Password);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(credentials.Password);

            administrator?.ToAdminDto(isLogged, hashedPassword!);
            await _administratorRepository.UpdateAsync(administrator!, token);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when trying to get an administrator by credentials: {e}", e);
            throw new EmployeeAccountingException("Error when trying to get an administrator by credentials", e);
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

    public async Task<bool> LoginAsync(Credentials credentials, CancellationToken token)
    {
        if (credentials.UserName == null)
        {
            return false;
        }

        var administrator = await _administratorRepository.GetByCredentialsAsync(credentials.UserName, token);
        return administrator != null && BCrypt.Net.BCrypt.Verify(credentials.Password, administrator.Password);
    }

    public async Task InitializeAdmin(CancellationToken token)
    {
        var admins = await _administratorRepository.ListAsync(token);
        if (!admins.Any())
        {
            var admin = new AdministratorDto()
            {
                Name = "admin",
                LastName = "admin",
                Login = "admin",
                Password = _serviceKeys?.ServiceKeys?.FirstAdminPass,
                OldPassword = _serviceKeys?.ServiceKeys?.FirstAdminPass,
                RegistrationTime = DateTimeOffset.UtcNow
            };

            var entity = _mapper.Map<Administrator>(admin);
            await _administratorRepository.AddAsync(entity, token);
        }
    }
}