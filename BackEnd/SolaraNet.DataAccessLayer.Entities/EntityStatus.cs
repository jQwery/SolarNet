namespace SolaraNet.DataAccessLayer.Entities
{
    /// <summary>
    /// Потому что нельзя ничего удалять из БД.
    /// </summary>
    public enum EntityStatus
    {
        Created, // только что создано
        Active, // видно всем
        Deleted, // удалено/скрыто, в любом случае можно воскресить из "мёртвых"
        DeletedByAdmin, // удалено администратором
        Rejected // отвергнуто
    }
}