using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
namespace SolaraNet.DAL.Testing
{
    class TestUnderCategoriesDB
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void CreateUnderCategorie()
        {
            var services = new ServiceCollection(); // Затычка, чтобы произвести тест, aka moq объект
            DBContextInstaller.ConfigureDbContext(services);
            SolaraNetDBContext db = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>());
            //db.UnderCategories.Add(new DBUnderCategory() { UnderCategoryTitle = "toyota", UnderCategoryLink = "htttp://..." , DBCategory = 1});
            db.SaveChanges();
            using (SolaraNetDBContext newContext = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>()))
            {

               // List<DBUnderCategory> underCategorieList = newContext.UnderCategories.ToList();
                //Assert.AreEqual("toyota", underCategorieList.Find(x => x.DBCategory == 1).UnderCategoryTitle);
                //Assert.AreEqual("htttp://...", underCategorieList.Find(x => x.DBCategory == 1).UnderCategoryLink);
            }
        }
    }
}