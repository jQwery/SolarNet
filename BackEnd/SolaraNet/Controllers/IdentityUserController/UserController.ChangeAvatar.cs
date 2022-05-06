using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.ApiExceptions;
using SolaraNet.Models;

namespace SolaraNet.Controllers.IdentityUserController
{
    public partial class UserController
    {
        [HttpPost("change-avatar")]
        [Authorize]
        public async Task<IActionResult> ChangeAvatar([FromBody]ChangeAvatarModel model, CancellationToken cancellationToken)
        {
            try
            {
                var changeResult = await _identityService.ChangeAvatar(model.AvatarLink, cancellationToken);
                if (!changeResult.Success)
                {
                    throw new NotChangedAvatarException(changeResult.GetErrors());
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}