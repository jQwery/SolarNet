using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SolaraNet.Models.SpecialModels
{
    public class ConfirmationModel
    {
        [Required]
        [NotNull]
        [MinLength(6)]
        [MaxLength(256)]
        public string UserName { get; set; }
        [Required]
        [NotNull]
        [MinLength(4)]
        [MaxLength(4)]
        public string Code { get; set; }
    }
}