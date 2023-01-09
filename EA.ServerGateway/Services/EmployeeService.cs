using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using AutoMapper;
using EA.Repository.Contracts;
using EA.Repository.Entities;
using EA.ServerGateway.Contracts;
using EA.ServerGateway.Dto;

namespace EA.ServerGateway.Services;

public class EmployeeService : IEmployeeService
{
    private const string FolderName = "EmployeePhoto";

    private static readonly string TrainerDataPath = Path.GetDirectoryName(
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

    //TODO need to test
    public async Task<EmployeeDto?> AddAsync(EmployeeDto employee, CancellationToken cancellationToken)
    {
        SaveImage(employee);
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

    private static void SaveImage(EmployeeDto employee)
    {
        try
        {
            var buffer = Convert.FromBase64String(employee.Photo);

            using var ms = new MemoryStream(buffer);
            var image = Image.FromStream(ms);
            image.Save($"{employee.PhotoName}.png", ImageFormat.Png);
        }
        catch (Exception e)
        {
            throw new ApplicationException("Error while saving image file into server folder!", e);
        }
    }

    private static void CreateFolder()
    {
        try
        {
            if (!Directory.Exists(TrainerDataPath))
            {
                Directory.CreateDirectory(FolderName);
            }
        }
        catch (Exception e)
        {
            throw new ApplicationException("Error while creating the folder", e);
        }
    }
}