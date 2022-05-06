using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SolaraNet.Controllers.IdentityUserController
{
    public partial class UserController
    {
        [HttpGet("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> Confirm(string userId, string token)
        {
            try
            {
                var isSuccessful = await _identityService.ConfirmEmail(userId, token);
                if (isSuccessful.Success)
                {
                    return RedirectPermanent("http://solaranet.ru");
                }
                _logger.LogError("Неправильный токен или идентификатор пользователя");
                return BadRequest("Неправильный токен или идентификатор пользователя");
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}