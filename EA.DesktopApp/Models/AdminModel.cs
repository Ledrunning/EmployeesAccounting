using System;

namespace EA.DesktopApp.Models
{
    public class AdminModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public DateTimeOffset RegistrationTime { get; set; }
    }
}