namespace SolaraNet.BusinessLogic.Contracts.Models
{
    public class RejectionModel
    {
        public int AdvertismentId { get; set; }
        /// <summary>
        /// Причина удаления
        /// </summary>
        public string DeleteReason { get; set; }
    }
}