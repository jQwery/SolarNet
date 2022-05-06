namespace SolaraNet.BusinessLogic.Contracts.Models
{
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; } // текущий пароль
        public string NewPassword { get; set; } // новый пароль
    }
}