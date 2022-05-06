namespace SolaraNet.BusinessLogic.Contracts.Models
{
    public class SimplePagination
    {
        /// <summary>
        /// Сколько объявлений на странице
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Сколько объявлений нужно пропустить (зависит от того, на какой мы странице)
        /// </summary>
        public int Offset { get; set; }
    }
}