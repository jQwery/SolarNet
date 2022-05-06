using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SolaraNet.AuthAPI.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [NotNull]
        [MaxLength(128)]
        [MinLength(6)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Почта обязательна")]
        [NotNull]
        [MaxLength(128)]
        [MinLength(6)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        [MaxLength(128)]
        [MinLength(6)]
        [NotNull]
        public string Password { get; set; }
        [Phone]
        [MaxLength(11)]
        [MinLength(11)]
        [NotNull]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Аватарка обязательна")]
        [MaxLength(256)]
        [MinLength(6)]
        [NotNull]
        public string AvatarLink { get; set; }
        public Roles Role { get; set; }

    }
}
