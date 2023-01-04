using System.ComponentModel.DataAnnotations;

namespace EA.Services.Models;

public class EmployeeDto : BaseModelDto
{
    [Required(ErrorMessage = "Enter the name")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Enter the last name")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Enter the department")]
    public string? Department { get; set; }

    public DateTimeOffset DateTime { get; set; }

    public string? PhotoPath { get; set; }
}