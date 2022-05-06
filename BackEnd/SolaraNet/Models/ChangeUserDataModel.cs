using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SolaraNet.Models
{
    public class ChangeUserDataModel
    {
        [AllowNull]
        public string? Name { get; set; }
        [NotNull]
        [Required(ErrorMessage = "Нужно ввести текущий пароль, это обязательно")]
        [MinLength(6)]
        public string CurrentPassword { get; set; }
        [AllowNull]
        public string? NewPassword { get; set; }
        [AllowNull]
        public string? NewEmail { get; set; }
        [AllowNull]
        public string? Phone { get; set; }
    }
}