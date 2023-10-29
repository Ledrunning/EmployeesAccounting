using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using EA.Common.Exceptions;
using EA.Repository.Contracts;
using EA.Repository.Entities;
using EA.Services.Configuration;
using EA.Services.Contracts;
using EA.Services.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EA.Services.Services;

public class AdministratorService : IAdministratorService
{
    private readonly IAdministratorRepository _administratorRepository;
    private readonly ConfigurationService _config;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IMapper _mapper;
    private readonly ServiceKeys? _serviceKeys;

    public AdministratorService(IMapper mapper,
        ILogger<EmployeeService> logger,
        IAdministratorRepository administratorRepository,
        ConfigurationService config)
    {
        _mapper = mapper;
        _logger = logger;
        _administratorRepository = administratorRepository;
        _config = config;
        _serviceKeys = _config.LoadConfiguration();
    }

    public async Task AddAsync(AdministratorDto admin, CancellationToken cancellationToken)
    {
        try
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(admin.Password);
            var currentAdmin = admin.Password = hashedPassword;

            var entity = _mapper.Map<Administrator>(currentAdmin);
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

    public async Task<bool> LoginAsync(string username, string password, CancellationToken token)
    {
        var administrator = await _administratorRepository.GetByCredentialsAsync(username, token);
        return administrator != null && BCrypt.Net.BCrypt.Verify(password, administrator.Password);
    }

    public Task<AdministratorDto?> GetByCredentialsAsync(string login, string pass, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<AdministratorDto?> GetByCredentialsAsync(string login, CancellationToken token)
    {
        try
        {
            var administrator = await _administratorRepository.GetByCredentialsAsync(login, token);
            return _mapper.Map<AdministratorDto>(administrator);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when trying to get an administrator by credentials: {e}", e);
            throw new EmployeeAccountingException("Error when trying to get an administrator by credentials", e);
        }
    }

    public string GenerateJwtToken(string username)
    {
        var secret = _serviceKeys?.Keys?.JwtSecretKey;
        if (secret == null)
        {
            return string.Empty;
        }

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username)
        };

        var tokenOptions = new JwtSecurityToken(
            "https://yourdomain.com",
            "https://yourdomain.com",
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);

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
                Password = _serviceKeys?.Keys?.FirstAdminPass,
                OldPassword = _serviceKeys?.Keys?.FirstAdminPass
            };

            var entity = _mapper.Map<Administrator>(admin);
            await _administratorRepository.AddAsync(entity, token);
        }
    }
}