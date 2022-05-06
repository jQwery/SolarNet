using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SolaraNet.Models.SpecialModels
{
    /// <summary>
    /// Модель для обновления модели
    /// </summary>
    public class UpdateAdvertismentModel
    {
        [NotNull]
        [Required(ErrorMessage = "Требуется указать id редактируемого объявления")]
        public int AdvertismentId { get; set; }
        [NotNull]
        [Required(ErrorMessage = "Заголовок не может быть пустым")]
        [MaxLength(256)]
        [MinLength(1)]
        public string AdvertismentTitle { get; set; }
        [NotNull]
        [Required(ErrorMessage = "Описание не может быть пустым")]
        [MaxLength(600)]
        [MinLength(1)]
        public string Description { get; set; }
        [NotNull]
        [Required(ErrorMessage = "Нужно указать новую цену для объявления")]
        public decimal Price { get; set; }
        [NotNull]
        [Required(ErrorMessage = "Нужно указать новые или старые картинки")]
        public List<string> Images { get; set; }
        [NotNull]
        [Required(ErrorMessage = "Нужно указать новую или оставить старую категорию для объявления")]
        public int CategoryId { get; set; }
        [NotNull]
        [Required(ErrorMessage = "Нужно указать новый или оставить старый город для объявления")]
        public string City { get; set; }
    }
}