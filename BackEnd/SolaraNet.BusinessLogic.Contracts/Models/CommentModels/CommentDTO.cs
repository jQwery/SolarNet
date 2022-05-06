using System;
using System.Collections.Generic;
using System.Text;

namespace SolaraNet.BusinessLogic.Contracts.Models
{
    public class CommentDTO
    {
        public int? Id { get; set; }
        public string CommentText { get; set; }
        public string UserName { get; set; }
        public string AvatarLink { get; set; }
    }
}
