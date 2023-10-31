using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EA.Repository.Entities;

public record Administrator : BaseEntity
{
    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Login { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar")]
    public string? Password { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar")]
    public string? OldPassword { get; set; }

    public DateTimeOffset RegistrationTime { get; set; }
}