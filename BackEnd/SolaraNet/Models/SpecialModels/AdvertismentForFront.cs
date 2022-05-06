using System.Collections.Generic;

namespace SolaraNet.Models.SpecialModels
{
    public class AdvertismentForFront
    {
        public int Id { get; set; }
        public string AdvertismentTitle { get; set; } // что-то по типу заголовка
        public string Description { get; set; } // Описание объявления
        public decimal Price { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public List<string> Images { get; set; }
        public int CategoryId { get; set; }
        public string City { get; set; }
        public string UserId { get; set; }
        public string UserPhone { get; set; }
        public string DeleteReason { get; set; }
        public string Status { get; set; }
        public int CommentsCount { get; set; }
    }
}