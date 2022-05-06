using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.BusinessLogic.Implementations;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BLL.Testing
{
    [TestClass]
    public class PaginationTest
    {
        /// <summary>
        /// Метод для тестирования пагинации с учётом большой вложенности категорий
        /// </summary>
        [TestMethod]
        public void TesingGetPaged()
        {
            List<DBCategory> categories = new List<DBCategory>();
            categories.Add(new DBCategory()
            {
                Id = 1
            });
            categories.Add(new DBCategory()
            {
                Id = 2,
                ParentId = 1,
                Parent = categories[0]
            });
            categories.Add(new DBCategory()
            {
                Id = 3,
                ParentId = 2,
                Parent = categories[1]
            });
            categories.Add(new DBCategory()
            {
                Id = 4,
                ParentId = 3,
                Parent = categories[2]
            });
            categories[0].UnderCategories = new List<DBCategory>();
            categories[0].UnderCategories.Add(categories[1]);
            categories[1].UnderCategories = new List<DBCategory>();
            categories[1].UnderCategories.Add(categories[2]);
            categories[2].UnderCategories = new List<DBCategory>();
            categories[2].UnderCategories.Add(categories[3]);
            Assert.AreEqual(4, GetPaged(categories).Result.Result);

        }

        /// <summary>
        /// Так как стандартный метод требует слишком много связей и очень сильно завязан на репозитории, нужно создать похожий метод, повторяющий его действия
        /// </summary>
        private async Task<OperationResult<int>> GetPaged(List<DBCategory> categories,
            CancellationToken cancellationToken = default)
        {
            int countAdvertisment = 0;
            int indexOfRecursion = 0;
            List<int> recursionCountList = new List<int>();
            GetAdvertismentsOfUnderCategory(categories);

            void GetAdvertismentsOfUnderCategory(List<DBCategory> categories )
            {
                foreach (var category in categories)
                {
                    recursionCountList.Add(indexOfRecursion);
                    AddUnderCategoriesAdvertisments(category, categories.Count());
                    if (category.UnderCategories!=null)
                    {
                        GetAdvertismentsOfUnderCategory(category.UnderCategories);
                    }
                }
            }

            void AddUnderCategoriesAdvertisments(DBCategory category, int recursionTimes)
            {
                if (recursionCountList[indexOfRecursion] >= recursionTimes)
                {
                    return;
                }
                countAdvertisment++; // будем считать, что тут каждый раз гарантированно найдётся какое-то объявление, потому что это ни на что не влияет
                recursionCountList[indexOfRecursion]++;
            }
            return OperationResult<int>.Ok(countAdvertisment);
        }
    }

    // P.S. По итогу был обнаружен баг и найдено его решение
}
