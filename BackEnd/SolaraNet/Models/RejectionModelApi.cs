using System.Diagnostics.CodeAnalysis;

namespace SolaraNet.Models
{
    public class RejectionModelApi
    {
        [NotNull]
        public int AdvertismentId { get; set; }
        [NotNull]
        public string DeleteReason { get; set; } // причина удаления объявления
    }
}