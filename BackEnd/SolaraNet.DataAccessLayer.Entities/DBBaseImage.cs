namespace SolaraNet.DataAccessLayer.Entities
{
    /// <summary>
    /// Что-то по типу абстрактного класса для картинок. Он содержит все базовые параметры для изображений.
    /// </summary>
    public class DBBaseImage
    {
        public int Id { get; set; }
        public string ImageLink { get; set; }
        public EntityStatus Status { get; set; }
    }
}