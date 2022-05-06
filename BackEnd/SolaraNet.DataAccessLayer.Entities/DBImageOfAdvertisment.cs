using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolaraNet.DataAccessLayer.Entities
{
    public class DBImageOfAdvertisment:DBBaseImage
    {
        public virtual DBAdvertisment Advertisment {get;set; }
    }
   

}
