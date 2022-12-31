namespace EA.ServerGateway.Models;

public class EmployeeDto : BaseModelDto
{
    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Department { get; set; }

    public DateTimeOffset DateTime { get; set; }

    public string? PhotoPath { get; set; }
}