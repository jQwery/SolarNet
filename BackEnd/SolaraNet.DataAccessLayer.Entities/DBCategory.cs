using System;
using System.Collections.Generic;
using System.Text;

namespace SolaraNet.DataAccessLayer.Entities
{
    public class DBCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public virtual List<DBCategory> UnderCategories { get; set; }
        public virtual List<DBAdvertisment> Advertisments { get; set; }
        public int? ParentId { get; set; } // id родительской категории, может быть Null, так как не каждая категория будет являться подкатегорией для какой-то категории
        public virtual DBCategory Parent { get; set; } // родительская категория
    }
}
