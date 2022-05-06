using System;
using System.Linq.Expressions;

namespace SolaraNet.DataAccessLayer.Entities
{
    /// <summary>
    /// Модель для пагинации, то есть для метода GetPaged()
    /// </summary>
    public class PaginationModel
    {
        /// <summary>
        /// Предикат, который далее подставится в LINQ запрос Where
        /// </summary>
        public Expression<Func<DBAdvertisment, bool>> PredicateWhere { get; set; }
        /// <summary>
        /// Смещение, то есть это число, которое обозначает количество объявлений, которые нужно пропустить. Вот, например, мы находимся на 10 странице, на одной странице выводится по 12 объявлений, это означает, что нужно 9*12. По сути, тут это не важно. Это число должно прийти с фронта.
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// Сколько объявлений нужно вывести
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Если стоит true, то нужно произвести сортировку по УБЫВАНИЮ цены создания объявления, иначе - по возрастанию
        /// </summary>
        public bool ByDescendingCost { get; set; }
        /// <summary>
        /// Тут указывается, относительно чего должно происходить сортировка
        /// </summary>
        public Expression<Func<DBAdvertisment, object>> PredicateOrderBy { get; set; }
    }
}