using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EA.ServerGateway.Model
{
    /// <summary>
    /// Add-Migration Initial
    /// Update-Database
    /// </summary>
    public record Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите Фамилию!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите название отдела!")]
        public string Department { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public string Photo { get; set; }
    }
}