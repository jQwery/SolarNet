using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.Models
{
    /// <summary>
    /// По сути тут те же поля, что и в GetRequestAdvertismentsModel, за исключением некоторых полей
    /// </summary>
    public class ExpensiveAdvertismentCostModel
    {
        /// <summary>
        /// Id категории, для которой нужно найти максимальную цену. Может быть пустым полем
        /// </summary>
        [AllowNull]
        public int? IdCategory { get; set; }
        /// <summary>
        /// Возможно ограничение по дате
        /// </summary>
        [AllowNull]
        public DateTime? Date { get; set; }
        public bool OnlyWithPhoto { get; set; }
        /// <summary>
        /// Если стоит галочка "С комментариями", то это true, иначе false
        /// </summary>
        public bool OnlyWithComments { get; set; }
        /// <summary>
        /// Если true, значит производим сортировку по цене по УБЫВАНИЮ, иначе по возростанию
        /// </summary>
        public bool ByDescendingCost { get; set; }
        /// <summary>
        /// Символы, которые напечатал пользователь в строке поиска объявлений
        /// </summary>
        public string? KeyWords { get; set; } = "";
        /// <summary>
        /// Город, для которого требуется произвести поиск
        /// </summary>
        public string? City { get; set; }
    }
}