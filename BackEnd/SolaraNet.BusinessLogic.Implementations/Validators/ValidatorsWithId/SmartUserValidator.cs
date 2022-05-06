using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.Common.Validators;

namespace SolaraNet.BusinessLogic.Implementations.Validators
{
    public class SmartUserValidator<TDbUser> : IUserValidator<TDbUser> where TDbUser:DBUser
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TDbUser> manager, TDbUser user)
        {
            var validationNameResult = await ValidateUserName(user.UserName);
            if (!validationNameResult) 
                return await Task.FromResult(IdentityResult.Failed());
            
            return await Task.FromResult(IdentityResult.Success);
        }

        private async Task<bool> ValidateUserName(string userName)
        {
            var minimalNameLenght = 2; // потому что если ник из одного символа, это как-то печально
            var validationResult = await Task.FromResult(StringValidator.CheckString(userName));
            validationResult = (userName.Length >= minimalNameLenght) && validationResult;
            if (!validationResult)
            {
                return false;
            }

            return true;
        }
    }
}