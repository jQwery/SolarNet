using System.Threading.Tasks;
using SolaraNet.DataAccessLayer.Abstracts;

namespace SolaraNet.DataAccessLayer.EntityFramework.DBContext
{
    /// <summary>
    /// Этот класс нужен, чтобы без лишней суеты сохранить изменения в БД
    /// </summary>
    public class Saver:ISaver
    {
        private SolaraNetDBContext _dbContext;

        public Saver(SolaraNetDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveAllChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}