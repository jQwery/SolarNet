using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore;
using Isopoh.Cryptography.Argon2;

namespace SolaraNet.DataAccessLayer.EntityFramework.Repositories
{
    public class TelegramRepository:ITelegramRepository
    {
        private readonly SolaraNetDBContext _dbcontext;
        public TelegramRepository(SolaraNetDBContext dBContext)
        {
            _dbcontext = dBContext;
        }
        public async Task<IEnumerable<DBAdvertisment>> ListAdvertismentsByWord(string word)
        {
            IQueryable<DBAdvertisment> db = _dbcontext.Advertisments.Include(x => x.AdvertismentTitle.Contains(word));
          var advertisments = await db.ToListAsync();
           var  ads = new DBAdvertisment { Id=1,Description="dddd",AdvertismentTitle="lol"};
            return (IEnumerable<DBAdvertisment>) ads;
        }

    }
}
