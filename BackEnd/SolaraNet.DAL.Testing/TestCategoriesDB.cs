using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;

namespace SolaraNet.DAL.Testing
{
    class TestCategoriesDB
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void CreateCategorie()
        {
            var services = new ServiceCollection(); // Затычка, чтобы произвести тест, aka moq объект
            DBContextInstaller.ConfigureDbContext(services);
            SolaraNetDBContext db = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>());
            db.Categories.Add(new DBCategory() {CategoryName="Cars", });
            db.SaveChanges(); 
            using (SolaraNetDBContext newContext = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>()))
            {

                List<DBCategory> categoryList = newContext.Categories.ToList();
                Assert.AreEqual("Cars", categoryList.Find(x => x.Id == 1).CategoryName);
            }
        }
       
       
    }
    
}
