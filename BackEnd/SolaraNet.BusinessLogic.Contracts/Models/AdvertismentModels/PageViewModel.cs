using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Implementations
{
    public class PageViewModel
    {
        public CategoryDTO? Category { get; set; } 
        public int AdvertismentPerPage { get; set; }
        /// <summary>
        /// Здесь надо немножко пояснить: это ключевые слова или даже символы, которые должны быть в объявлении. То есть сфера применения такова: пользователь на сайте вбивает поиск, пишет там "недорого" и по этому ключевому слову производится поиск, чтобы были совпадения. В общем, это нужно для поиска
        /// </summary>
        public string? KeyWords { get; set; }
        public int Offset { get; set; }
        /// <summary>
        /// Сортировать по дате публикации (может возникнуть необходимость искать объявление по конкретной дате, вот для этого нужно это поле)
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Минимальная цена
        /// </summary>
        public decimal MinimumCost { get; set; }
        /// <summary>
        /// Максимальная цена
        /// </summary>
        public decimal MaximumCost { get; set; }
        /// <summary>
        /// Если стоит галочка "Только с фото", то это true, иначе false
        /// </summary>
        public bool OnlyWithPhoto { get; set; }
        /// <summary>
        /// Если стоит галочка "С комментариями", то это true, иначе false
        /// </summary>
        public bool OnlyWithComments { get; set; }
        /// <summary>
        /// Если true, значит производим сортировку по цене по УБЫВАНИЮ, иначе по возростанию
        /// </summary>
        public bool ByDescending { get; set; }
        /// <summary>
        /// Город, по которому требуется осуществить поиск
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// Статус объявлений. Например, если Active, то это уже одобренный администратором, а если Created, то только что созданные, ещё не одобренные администратором
        /// </summary>
        public EntityStatus RequeredStatus { get; set; }
        /// <summary>
        /// Сортировка по дате
        /// </summary>
        public bool SortByDate { get; set; }
        /// <summary>
        /// Сортировка по цене
        /// </summary>
        public bool SortByCost { get; set; }
        public PageViewModel(CategoryDTO? category, int advertismentPerPage, int offset, DateTime? date, decimal minimumCost, decimal maximumCost, bool onlyWithPhoto, bool onlyWithComments, bool byDescending, string? keyWords, string? city, bool sortByDate, bool sortByCost, EntityStatus requeredStatus=EntityStatus.Active)
        {
            Category = category;
            AdvertismentPerPage = advertismentPerPage;
            Offset = offset;
            Date = date;
            MinimumCost = minimumCost;
            MaximumCost = maximumCost;
            OnlyWithPhoto = onlyWithPhoto;
            OnlyWithComments = onlyWithComments;
            ByDescending = byDescending;
            KeyWords = keyWords;
            City = city;
            SortByDate = sortByDate;
            SortByCost = sortByCost;
            RequeredStatus = requeredStatus;
        }
    }
}
