namespace EA.ServerGateway.Models;

public class AdministratorDto : BaseModelDto
{
    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? OldPassword { get; set; }

    public DateTimeOffset RegistrationTime { get; set; }
}