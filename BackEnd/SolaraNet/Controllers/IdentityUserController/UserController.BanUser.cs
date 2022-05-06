using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.ApiExceptions;

namespace SolaraNet.Controllers.IdentityUserController
{
    public partial class UserController
    {
        [HttpDelete("id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BanUser(string id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userService.BanUser(id, cancellationToken);
                if (!result.Success)
                {
                    throw new NotBannedUserException(result.GetErrors());
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