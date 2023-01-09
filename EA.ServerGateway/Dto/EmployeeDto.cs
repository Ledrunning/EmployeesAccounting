using System.ComponentModel.DataAnnotations;

namespace EA.ServerGateway.Dto;

public class EmployeeDto : BaseModelDto
{
    [Required(ErrorMessage = "Enter the name")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Enter the last name")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Enter the department")]
    public string? Department { get; set; }

    public DateTimeOffset DateTime { get; set; }

    public string? Photo { get; set; }
    public string? PhotoName { get; set; }
}