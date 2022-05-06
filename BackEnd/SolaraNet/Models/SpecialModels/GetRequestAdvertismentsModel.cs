using System;
using System.Diagnostics.CodeAnalysis;
using SolaraNet.BusinessLogic.Contracts;

namespace SolaraNet.Models.SpecialModels
{
    public class GetRequestAdvertismentsModel
    {
        /// <summary>
        /// Количество возвращаемых объявлений
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Смещение, начиная с которого возвращаются объявления
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// Id категории
        /// </summary>
        [AllowNull]
        public int? IdCategory { get; set; }
        /// <summary>
        /// Сортировать по дате публикации, формат таков 2021-03-24T00:00:00.000Z
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Минимальная цена
        /// </summary>
        public decimal? MinimumCost { get; set; }
        /// <summary>
        /// Максимальная цена
        /// </summary>
        public decimal? MaximumCost { get; set; }
        /// <summary>
        /// Если стоит галочка "Только с фото", то это true, иначе false
        /// </summary>
        public bool OnlyWithPhoto { get; set; }
        /// <summary>
        /// Если стоит галочка "С комментариями", то это true, иначе false
        /// </summary>
        public bool OnlyWithComments { get; set; }
        /// <summary>
        /// Если true, значит производим сортировку по дате по УБЫВАНИЮ, иначе по возростанию
        /// </summary>
        public bool ByDescending { get; set; }
        /// <summary>
        /// Символы, которые напечатал пользователь в строке поиска объявлений
        /// </summary>
        public string? KeyWords { get; set; } = "";
        /// <summary>
        /// Город, для которого требуется произвести поиск
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// Сортировать по цене
        /// </summary>
        public bool SortByCost { get; set; }
        /// <summary>
        /// Сортировать по дате
        /// </summary>
        public bool SortByDate { get; set; }
    }
}