using System.Collections.Generic;
using System;
namespace SolaraNet.DataAccessLayer.Entities
{
    public class DBAdvertisment
    {
        public int Id { get; set; }
        public virtual DBUser User { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsPayed { get; set; }
        public string AdvertismentTitle { get; set; } // что-то по типу заголовка
        public string Description { get; set; } // Описание объявления
        public decimal Price { get; set; }
        public EntityStatus Status { get; set; }
        public virtual List<DBImageOfAdvertisment> Image { get; set; }
        public virtual DBCategory DBCategory { get; set; }
        public int DBCategoryId { get; set; }
        public virtual List<DBComment> Comments { get; set; } // комментарии к этому объявлению
        /// <summary>
        /// Индетификатор пользователя
        /// </summary>        
        public string UserId { get; set; }
        /// <summary>
        /// Город, в котором продаётся этот товар
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Причина удаления объявления на случай, если объявление удалено/отвергнуто администратором
        /// </summary>
        public string DeleteReason { get; set; }

    }
}