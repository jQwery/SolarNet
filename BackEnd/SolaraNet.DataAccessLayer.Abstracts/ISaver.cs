using System.Threading.Tasks;

namespace SolaraNet.DataAccessLayer.Abstracts
{
    public interface ISaver
    {
        /// <summary>
        /// Сохранение всех изменений в БД
        /// </summary>
        /// <returns></returns>
        Task SaveAllChanges();
    }
}