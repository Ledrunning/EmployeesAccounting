using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using AutoMapper;
using EA.Common.Exceptions;
using EA.Repository.Contracts;
using EA.Repository.Entities;
using EA.Services.Contracts;
using EA.Services.Dto;

namespace EA.Services.Services;

public class EmployeeService : IEmployeeService
{
    private const string FolderName = "EmployeePhoto";

    private static readonly string PhotoDataPath = Path.GetDirectoryName(
                                                       Assembly.GetExecutingAssembly().Location) +
                                                   $"\\{FolderName}";

    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        CreateFolder();
    }

    public async Task<IReadOnlyList<EmployeeDto?>> GetAllWithPhotoAsync(CancellationToken cancellationToken)
    { 
        var employees = await _employeeRepository.ListAsync(cancellationToken);

        foreach (var employee in employees)
        {
            if (employee.PhotoPath == null)
            {
                continue;
            }

            await using var fileStream = File.Open(employee.PhotoPath, FileMode.Open);
            {
                //TODO: как тут быть?
            }
        }

        //TODO: как тут быть?
        return _mapper.Map<IReadOnlyList<EmployeeDto>>(employees);
    }

    public async Task<IReadOnlyList<EmployeeDto?>> GetAllAsync(CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.ListAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<EmployeeDto>>(employee);
    }

    public async Task<EmployeeDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<EmployeeDto>(employee);
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
            throw new EmployeeAccountingException("Failed to add employee into database!", e);
        }
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

    private static void SaveImage(EmployeeDto employee)
    {
        try
        {
            if (employee.Photo == null)
            {
                throw new EmployeeAccountingException("Error while saving image file into server folder!");
            }

            using var memoryStream = new MemoryStream(employee.Photo);
            var image = Image.FromStream(memoryStream);
            image.Save($"{PhotoDataPath}\\{employee.Name}", ImageFormat.Jpeg);
        }
        catch (Exception e)
        {
            throw new EmployeeAccountingException("Error while saving image file into server folder!", e);
        }
    }

    private static void CreateFolder()
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
            throw new EmployeeAccountingException("Error while creating the folder", e);
        }
    }
}