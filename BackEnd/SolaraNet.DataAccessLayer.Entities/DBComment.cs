using System;
using System.Collections.Generic;
using System.Text;

namespace SolaraNet.DataAccessLayer.Entities
{
    public class DBComment
    {
        public int Id { get; set; }
        public virtual DBUser User { get; set; }
        public virtual DBAdvertisment Advertisment { get; set; }
        public int AdvertismentId { get; set; }
        public string CommentText { get; set; }
        public DateTime PublicationDate { get; set; }
        public EntityStatus CommentStatus { get; set; }
        /// <summary>
        /// Индетификатор пользователя
        /// </summary>        
        public string UserId { get; set; }
    }
}
