using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using AutoMapper;
using EA.Common.Exceptions;
using EA.Repository.Contracts;
using EA.Repository.Entities;
using EA.Services.Contracts;
using EA.Services.Dto;
using EA.Services.Extension;
using Microsoft.Extensions.Logging;

namespace EA.Services.Services;

public class EmployeeService : IEmployeeService
{
    private const string FolderName = "EmployeePhoto";

    private static readonly string PhotoDataPath = Path.GetDirectoryName(
                                                       Assembly.GetExecutingAssembly().Location) +
                                                   $"\\{FolderName}";

    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _logger = logger;
        CreateFolder();
    }

    public async Task<IReadOnlyList<EmployeeDto?>> GetAllWithPhotoAsync(CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.ListAsync(cancellationToken);
        var employeeDtos = new List<EmployeeDto>();

        foreach (var employee in employees)
        {
            byte[]? photoData = null;

            if (!string.IsNullOrEmpty(employee.PhotoPath) && File.Exists(employee.PhotoPath))
            {
                try
                {
                    photoData = await File.ReadAllBytesAsync(employee.PhotoPath, cancellationToken);
                }
                catch(Exception e)
                {
                    _logger.LogError("Error while reading image file: {e}", e);
                    throw new EmployeeAccountingException("Error while reading image file: ", e);
                }
            }

            employeeDtos.Add(employee.ToEmployeeDto(photoData));
        }

        return employeeDtos;
    }
    
    public async Task<IReadOnlyList<EmployeeDto?>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _employeeRepository.ListAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<EmployeeDto>>(employee);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when trying to get all employee: {e}", e);
            throw new EmployeeAccountingException("Error when trying to get all employee", e);
        }
    }

    public async Task<EmployeeDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<EmployeeDto>(employee);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when trying to get an employee by Id: {e}", e);
            throw new EmployeeAccountingException("Error when trying to get an employee by Id", e);
        }
    }

    public async Task<string?> GetNameByIdAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            return $"{employee?.Name} {employee?.LastName}";
        }
        catch (Exception e)
        {
            _logger.LogError("Error when trying to get an employee name by Id: {e}", e);
            throw new EmployeeAccountingException("Error when trying to get an employee name by Id", e);
        }
    }

    public async Task AddAsync(EmployeeDto employee, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Employee>(employee);
            await _employeeRepository.AddAsync(entity, cancellationToken);
            SaveImage(employee);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to add an employee to the database!: {e}", e);
            throw new EmployeeAccountingException("Failed to add an employee to the database!", e);
        }
    }

    public async Task UpdateAsync(EmployeeDto employee, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Employee>(employee);
            await _employeeRepository.UpdateAsync(entity, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to update an employee in database!: {e}", e);
            throw new EmployeeAccountingException("Failed to update an employee in database!", e);
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            await _employeeRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete an employee in database!: {e}", e);
            throw new EmployeeAccountingException("Failed to delete an employee in database!", e);
        }
    }

    private void SaveImage(EmployeeDto employee)
    {
        try
        {
            if (employee.Photo == null)
            {
                throw new EmployeeAccountingException("Error while saving image file into server folder!");
            }

            using var memoryStream = new MemoryStream(employee.Photo);
            var image = Image.FromStream(memoryStream);
            image.Save($"{PhotoDataPath}\\{employee.PhotoName}", ImageFormat.Jpeg);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while saving an image file into server folder!: {e}", e);
            throw new EmployeeAccountingException("Error while saving an image file into server folder!", e);
        }
    }

    private void CreateFolder()
    {
        try
        {
            if (!Directory.Exists(PhotoDataPath))
            {
                Directory.CreateDirectory(PhotoDataPath);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error while creating a folder: {e}", e);
            throw new EmployeeAccountingException("Error while creating a folder", e);
        }
    }
}