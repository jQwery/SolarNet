using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SolaraNet.BusinessLogic.Abstracts
{
    public interface ITelegramService
    {
       IEnumerable<AdvertismentDTO> ListAdvertismentsByWord(string word);
    }
}
