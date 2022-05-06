using SolaraNet.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SolaraNet.DataAccessLayer.Abstracts
{
    public interface ITelegramRepository
    {
        Task<IEnumerable<DBAdvertisment>> ListAdvertismentsByWord(string word);
    }
}
