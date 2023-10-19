using EA.Repository.Entities;
using EA.Services.Dto;

namespace EA.Services.Extension;

public static class EmployeeExtensions
{
    public static EmployeeDto ToEmployeeDto(this Employee employee, byte[]? photoData = null)
    {
        var dto = new EmployeeDto
        {
            Id = employee.Id,
            Name = employee.Name,
            LastName = employee.LastName,
            Department = employee.Department,
            DateTime = employee.DateTime,
            PhotoName = employee.PhotoName,
        };

        if (photoData != null)
        {
            dto.Photo = photoData;
        }

        return dto;
    }
}