using System;
using System.ComponentModel.DataAnnotations;

namespace EA.DesktopApp.Models
{
    /// <summary>
    ///     Model for Web Api client
    ///     Need to point to System.ComponentModel.DataAnnotations;
    /// </summary>
    public class EmployeeModel
    {
        [Required(ErrorMessage = "Enter the name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter the last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter the department")]
        public string Department { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public string Photo { get; set; }
        public string PhotoName { get; set; }

    }
}