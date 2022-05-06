using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.DataAccessLayer.EntityFramework.Repositories;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore;


namespace SolaraNet.DataAccessLayer.EntityFramework.Repositories
{
    public partial class AdvertismentRepository : IAdvertismentRepository
    {
        private readonly SolaraNetDBContext _dbContext; 

        public AdvertismentRepository(SolaraNetDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task CreateNewAdvertisment(DBAdvertisment advertismentToBeAdded, CancellationToken cancellationToken)
        {
            if (!await IsAdvertismentTitleExist(advertismentToBeAdded.AdvertismentTitle))
            {
                await _dbContext.AddAsync(advertismentToBeAdded);
            }
            else
            {
                throw new ArgumentException("Объявление с таким заголовком уже есть, нельзя его добавЛЯТЬ");
            }
        }

        public async Task<bool> DeleteAdvertisment(int id, bool byAdmin, CancellationToken cancellationToken)
        {
            if (await IsAdvertismentExistAndActive(id, cancellationToken))
            {
                if (!byAdmin)
                {
                    GetAdvertismentById(id, cancellationToken).Result.Status = EntityStatus.Deleted;
                    return true;   
                }
                GetAdvertismentById(id, cancellationToken).Result.Status = EntityStatus.DeletedByAdmin;
                return true; 
            }

            return false;
        }

        public async Task<bool> UpdateAdvertisment(int id, DBAdvertisment newAdvertisment, CancellationToken cancellationToken)
        {
            if (await IsAdvertismentExists(id, cancellationToken))
            {
                DBAdvertisment dBAdvertisment = await GetAdvertismentById(id,cancellationToken);
                dBAdvertisment.AdvertismentTitle = newAdvertisment.AdvertismentTitle;
                dBAdvertisment.Description = newAdvertisment.Description;
                dBAdvertisment.DBCategoryId = newAdvertisment.DBCategoryId;
                dBAdvertisment.Price = newAdvertisment.Price;
                dBAdvertisment.City = newAdvertisment.City;
                dBAdvertisment.Status = EntityStatus.Created;
                if (newAdvertisment.Image!=null)
                {
                    dBAdvertisment.Image.Clear();
                    dBAdvertisment.Image = new List<DBImageOfAdvertisment>();
                    foreach (var image in newAdvertisment.Image)
                    {
                        dBAdvertisment.Image.Add(image);
                    }
                }

                return true;
            }

            return false;
        }

        public async Task<DBAdvertisment> GetAdvertismentById(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Advertisments.FirstOrDefaultAsync(p => p.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> AdvertismentsCount(Expression<Func<DBAdvertisment, bool>> predicate, CancellationToken cancellationToken)
        {
            var data = _dbContext.Set<DBAdvertisment>();
            return await data.Where(predicate).CountAsync(cancellationToken);
        }

        public async Task<List<DBAdvertisment>> GetPaged(PaginationModel model, CancellationToken cancellationToken)
        {
            if (model.PredicateWhere==null)
            {
                model.PredicateWhere = advertisment => true;
            }
            var data = _dbContext.Set<DBAdvertisment>();
            List<DBAdvertisment> advertismentsToBeReturned = new List<DBAdvertisment>();

            #region Проверки насчёт убывания и возрастания

            if (model.ByDescendingCost)
            {
                advertismentsToBeReturned = await data.Where(model.PredicateWhere).OrderByDescending(model.PredicateOrderBy).Skip(model.Offset).Take(model.Limit).ToListAsync(cancellationToken);
            }
            else if (!model.ByDescendingCost)
            {
                advertismentsToBeReturned = await data.Where(model.PredicateWhere).OrderBy(model.PredicateOrderBy).Skip(model.Offset).Take(model.Limit)
                    .ToListAsync(cancellationToken); 
            }

            #endregion
            int a = 5;
            return advertismentsToBeReturned;
        }

        private async Task<bool> IsAdvertismentExistAndActive(int id, CancellationToken cancellationToken)
        {
            var result = await Task.Run((() => GetAdvertismentById(id, cancellationToken)));
            var resultDelete = await Task.FromResult(result.Status);
            return result != null && resultDelete == EntityStatus.Active;
        }

        private async Task<bool> IsAdvertismentExists(int id, CancellationToken cancellationToken)
        {
            var result = await Task.Run((() => GetAdvertismentById(id, cancellationToken)));
            return result != null;
        }

        private async Task<bool> IsAdvertismentTitleExist(string titleOfAdvertisment)
        {
            var result = await Task.Run((() =>
                _dbContext.Advertisments.FirstOrDefaultAsync(a => a.AdvertismentTitle == titleOfAdvertisment)));
            return result!= null;
        }
    }
}
