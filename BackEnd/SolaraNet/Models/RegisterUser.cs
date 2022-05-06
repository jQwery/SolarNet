using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using SolaraNet.Models.SpecialModels;

namespace SolaraNet.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [NotNull]
        [MaxLength(128)]
        [MinLength(3)]
        public string? Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Почта обязательна")]
        [NotNull]
        [MinLength(3)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(8)]
        [NotNull]
        public string? Password { get; set; }
        [MaxLength(12)]
        [MinLength(11)]
        [NotNull]
        public string? Phone { get; set; }
        [MinLength(1)]
        [MaxLength(4)]
        [NotNull]
        public string? Code { get; set; }

    }
}
