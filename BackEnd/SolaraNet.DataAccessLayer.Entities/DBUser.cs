using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolaraNet.DataAccessLayer.Entities
{
    public class DBUser:IdentityUser // наследуемся от IdentityUser, чтобы использовать этот класс в IdentityFramework, потому как IdentityUser не полностью удовлетворяет нас
    {
        [ForeignKey("Avatars")]
        public virtual DBAvatar Avatar { get; set; } // аватарки пользователя, воооот это самое
        public EntityStatus Status { get; set; } // статус пользователя (забанен или не забанен)
        public virtual List<DBComment> Comments { get; set; } // комментарии, которые оставил пользователь
    }
}
