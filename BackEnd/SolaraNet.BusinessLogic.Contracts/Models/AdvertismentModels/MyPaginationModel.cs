namespace SolaraNet.BusinessLogic.Contracts.Models
{
    public class MyPaginationModel : SimplePagination
    {
        public bool HideDeleted { get; set; }
    }
}