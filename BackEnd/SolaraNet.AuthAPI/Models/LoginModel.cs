using System.ComponentModel.DataAnnotations;

namespace SolaraNet.AuthAPI.Models
{
    public class LoginModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email обязательно должен быть")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль обязательно должен быть")]
        [MaxLength(30)]
        [MinLength(6)]
        public string Password { get; set; }
    }
}