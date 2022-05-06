using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;

namespace SolaraNet.Models
{
    public class AddCommentModel
    {
        /// <summary>
        /// Id объявления, которому нужно добавить комментарий
        /// </summary>
        [NotNull]
        public int AdvertismentId { get; set; }
        /// <summary>
        /// Текст комментария
        /// </summary>
        [NotNull]
        [MinLength(3)]
        public string Text { get; set; }
    }
}