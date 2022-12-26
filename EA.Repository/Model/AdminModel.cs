using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EA.Repository.Model
{
    public record AdminModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
}
