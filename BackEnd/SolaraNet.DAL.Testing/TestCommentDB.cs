using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
namespace SolaraNet.DAL.Testing
{
    class TestCommentDB
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateNewComment()
        {
            var services = new ServiceCollection(); // Затычка, чтобы произвести тест, aka moq объект
            DBContextInstaller.ConfigureDbContext(services);
            SolaraNetDBContext db = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>());
            db.Comments.Add(new DBComment() {CommentText="good bike"});
            db.SaveChanges();
            using (SolaraNetDBContext newContext = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>()))
            {

                List<DBComment> commentList = newContext.Comments.ToList();
                Assert.AreEqual("good bike", commentList.Find(x => x.Id == 1).CommentText);
                
            }
        }
    }
}
