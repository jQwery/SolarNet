using System.Collections.Generic;
using System;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Contracts.Models
{
    public class AdvertismentDTO
    {
        public int AdvertismentID { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsPayed { get; set; }
        public decimal Price { get; set; }
        public bool IsNew { get; set; } //состояние товара
        public string AdvertismentTitle{ get; set; } // что-то по типу заголовка
        public string Description { get; set; } // Описание объявления
        public CategoryDTO Category { get; set; }
        public string UserId { get; set; }
        public string? UserName { get; set; } // имя пользователя, используется не всегда и не везде, потому AllowNull
        public string UserPhone { get; set; } // номер телефона пользователя
        public List<string> ImagesList { get; set; }
        public string City { get; set; }
        public string DeleteReason { get; set; } // причина удаления
        public EntityStatus? Status { get; set; }
        public int CommentCount { get; set; }
    }
    
}