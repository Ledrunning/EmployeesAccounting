using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EA.Repository.Entities;

/// <summary>
///     Add-Migration Initial
///     Update-Database
/// </summary>
public record Employee : BaseEntity
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