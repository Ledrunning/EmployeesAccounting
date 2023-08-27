namespace EA.Repository.Entities;

public record Employee : BaseEntity
{
    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Department { get; set; }

    public DateTimeOffset DateTime { get; set; }

    public string? PhotoPath { get; set; }
    public string? PhotoName { get; set; }
}