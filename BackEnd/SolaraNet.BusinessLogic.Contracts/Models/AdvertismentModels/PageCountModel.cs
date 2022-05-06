using System;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Contracts.Models
{
    public class PageCountModel
    {
        public int? IdCategory { get; set; }
        public int AdvertismentsPerPage { get; set; }
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
        /// Город, по которому требуется осуществить поиск
        /// </summary>
        public string City { get; set; }
        public string KeyWords { get; set; }
        public EntityStatus Status { get; set; }
    }
}