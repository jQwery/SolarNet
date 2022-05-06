namespace SolaraNet.BusinessLogic.Contracts.Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Image Avatar { get; set; }
        public string Role { get; set; }
    }
}