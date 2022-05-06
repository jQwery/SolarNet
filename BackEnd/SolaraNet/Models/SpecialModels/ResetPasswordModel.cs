using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolaraNet.Models.SpecialModels
{
    public class ResetPasswordModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
