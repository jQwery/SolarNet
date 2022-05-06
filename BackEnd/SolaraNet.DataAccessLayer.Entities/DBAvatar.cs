using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolaraNet.DataAccessLayer.Entities
{
    public class DBAvatar:DBBaseImage
    {
        public virtual DBUser User { get; set; }
        /// <summary>
        /// Индетификатор пользователя
        /// </summary>        
        public string UserId { get; set; }
    }
}
