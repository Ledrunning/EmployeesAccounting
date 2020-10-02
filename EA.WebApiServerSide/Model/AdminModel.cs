using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EA.WebApiServerSide.Model
{
    public class AdminModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите Фамилию!")]
        public string UserLastName { get; set; }

        [Required(ErrorMessage = "Введите логин!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите старый пароль!")]
        public string OldPassword { get; set; }

        public DateTimeOffset RegistrationTime { get; set; }
    }
}
