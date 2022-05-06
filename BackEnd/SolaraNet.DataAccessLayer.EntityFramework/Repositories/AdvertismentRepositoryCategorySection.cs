using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.DataAccessLayer.EntityFramework.Repositories
{
    public partial class AdvertismentRepository
    {
        public async Task<IEnumerable<DBCategory>> GetCategories(CancellationToken cancellationToken)
        {
            return await _dbContext.Categories.ToListAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Получить все подкатегории какой-то супер категории.
        /// </summary>
        /// <param name="idCategory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DBCategory>> GetUndercategoryOfCategoriesByCategoryId(int parentCategoryId,
            CancellationToken cancellationToken)
        {
            var undercategories = await _dbContext.Categories.Where(x => x.ParentId == parentCategoryId).ToListAsync(cancellationToken);
            return undercategories;
        }

        public async Task<DBCategory> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories.Where(x => x.Id == id).FirstAsync(cancellationToken);
        }
    }
}