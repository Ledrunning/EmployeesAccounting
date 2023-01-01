using System.ComponentModel.DataAnnotations;

namespace EA.ServerGateway.Models;

public class AdministratorDto : BaseModelDto
{
    [Required(ErrorMessage = "Enter the name")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Enter the last name")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Enter the login")]
    public string? Login { get; set; }

    [Required(ErrorMessage = "Enter the password")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Enter your old password")]
    public string? OldPassword { get; set; }

    public DateTimeOffset RegistrationTime { get; set; }
}