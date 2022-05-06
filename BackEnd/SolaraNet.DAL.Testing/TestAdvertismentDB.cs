using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
namespace SolaraNet.DAL.Testing
{
    class TestAdvertismentDB
    {
        [Test]
        public void Setup()
        {

        }
        public void CreateNewAdvertisment()
        {
            var services = new ServiceCollection();
            DBContextInstaller.ConfigureDbContext(services);
            SolaraNetDBContext db = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>());
            db.Advertisments.Add(new DBAdvertisment() { AdvertismentTitle = "Bike", Price = 1000, Description = "good bike for good boy" });
            db.SaveChanges();
            using (SolaraNetDBContext newContext = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>()))
            {
                List<DBAdvertisment>advertismentsList = newContext.Advertisments.ToList();
                Assert.AreEqual("Bike", advertismentsList.Find(x => x.Id == 1).AdvertismentTitle);
            }
        }
    }
}
