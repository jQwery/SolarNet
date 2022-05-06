using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SolaraNet.Models
{
    public class PaginationByUserIdModel:SimplePaginationModel
    {
        [NotNull]
        [Required(ErrorMessage = "Нужно ввести Id пользователя")]
        public string UserId { get; set; }
    }
}