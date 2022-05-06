using System;
using System.Threading;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Org.BouncyCastle.Math.EC.Rfc7748;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.BusinessLogic.Implementations;
using SolaraNet.BusinessLogic.Implementations.Mapper;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using Assert = NUnit.Framework.Assert;

namespace SolaraNet.BLL.Testing
{
    [TestClass]
    public class TestPaginationExtended
    {
        [TestMethod]
        public void GetPaggedCount()
        {
            Mock<IAdvertismentRepository> advertismentRepository = new();
            Mock<IUserRepository> userRepository = new();
            Mock<ISaver> saver = new();
            Mock<IIdentityService> identityService = new();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AdvertismentsMapper>();
                cfg.AddProfile<CategoryMapper>();
                cfg.AddProfile<CommentMapper>();
                cfg.AddProfile<ImagesMapper>();
                cfg.AddProfile<PaginationMapper>();
                cfg.AddProfile<UserMapper>();
            });
            Mock<IMapper> mapper = new();
            configuration.AssertConfigurationIsValid();
            advertismentRepository.Setup(x=>x.CreateNewAdvertisment(new DBAdvertisment {AdvertismentTitle = "Test", City = "Таганрог", Price = 200, PublicationDate = DateTime.Today, Status = EntityStatus.Active, DBCategoryId = 1}, CancellationToken.None));
            AdvertismentService service = new(advertismentRepository.Object, saver.Object, mapper.Object, identityService.Object, userRepository.Object);
            var pageModel = new PageCountModel() {AdvertismentsPerPage = 1, City = "", Date = DateTime.Today, IdCategory = null, KeyWords = "", MaximumCost = 3000, MinimumCost = 0, OnlyWithComments = false, OnlyWithPhoto = false, Status = EntityStatus.Active};
            Assert.AreEqual(1, service.GetPagesCount(pageModel, new CancellationToken()).Result.Result);
        }
    }
}