namespace EA.Repository.Entities;

/// <summary>
///     Add-Migration Initial
///     Update-Database
/// </summary>
public record Employee : BaseEntity
{
    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Department { get; set; }

    public DateTimeOffset DateTime { get; set; }

    public string? PhotoPath { get; set; }
}