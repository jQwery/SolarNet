using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;

namespace SolaraNet.DAL.Testing
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateNewUser()
        {
            //var services = new ServiceCollection(); // �������, ����� ���������� ����, aka moq ������
            //DBContextInstaller.ConfigureDbContext(services);
            //SolaraNetDBContext db = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>());
            //db.Users.Add(new DBUser() { Mail = "eduard.pirogov.2000@gmail.com", Name = "������� ������ ��������", PasswordHash = "8800553535, ����� ���������, ��� � ����-�� ��������", PhoneNumber = "+79781067697" });
            //db.SaveChanges(); // �������� ������ ����� � ��������� ���������
            //using (SolaraNetDBContext newContext = new SolaraNetDBContext(new DbContextOptions<SolaraNetDBContext>()))
            //{

            //    List<DBUser> userList = newContext.Users.ToList();
            //    Assert.AreEqual("������� ������ ��������", userList.Find(x => x.Id == 1.ToString()).Name);
            //}
        }
    }
}