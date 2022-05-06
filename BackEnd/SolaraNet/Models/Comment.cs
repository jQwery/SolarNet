using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolaraNet.Models
{
    public class Comment
    {
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public int AdvertismentID { get; set; }
        public int DaysAgo { get; set; } //сколько дней назад оставили коммент
    }
}
