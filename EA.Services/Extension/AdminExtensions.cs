using EA.Repository.Entities;
using EA.Services.Dto;

namespace EA.Services.Extension;

public static class AdminExtensions
{
    public static AdministratorDto ToAdminDto(this Administrator admin, bool isLogged, string encodedPassword)
    {
        var dto = new AdministratorDto
        {
            Id = admin.Id,
            Name = admin.Name,
            LastName = admin.LastName,
            Login = admin.Login,
            Password = encodedPassword,
            OldPassword = encodedPassword,
            RegistrationTime = admin.RegistrationTime,
            IsLogged = isLogged
        };

        return dto;
    }
}