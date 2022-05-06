using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SolaraNet.Models
{
    public class AdvertismentFromFront
    {
        public decimal Price { get; set; }
        public string AdvertismentTitle { get; set; } // что-то по типу заголовка
        public string Description { get; set; } // Описание объявления
        public List<string> Images { get; set; } // ссылки на картинки
        public int CategoryId { get; set; }
        public string City { get; set; }
    }
}
