namespace SolaraNet.Models.SpecialModels
{
    public class GetRequestCommentModel
    {
        /// <summary>
        /// Id объявления, для которого требуется получить комментарии
        /// </summary>
        public int AdvertismentId { get; set; }
        /// <summary>
        /// Количество возвращаемых объявлений
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Смещение, начиная с которого возвращаются объявления
        /// </summary>
        public int Offset { get; set; }
    }
}