using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.ApiExceptions;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models;

namespace SolaraNet.Controllers.IdentityUserController
{
    public partial class UserController
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUser user, CancellationToken cancellationToken)
        {
            try
            {
                decimal number;
                bool isNumberValid = decimal.TryParse(user.Phone, out number);
                if (!isNumberValid)
                {
                    return BadRequest("Невалидный номер телефона");
                }
                var removeResult = await Task.FromResult(CapchaGeneratorSimpleVersion.RemoveCapchaFromList(user.Code));
                if (!removeResult)
                {
                    return BadRequest("Неверная капча");
                }
                var result = await _userService.Register(_mapper.Map<UserDTO>(user), cancellationToken);
                if (!result.Success)
                {
                    _logger.LogError(result.GetErrors());
                    throw new NotAddedNewUser("Не удалось зарегистрировать пользователя. И вот почему: " + result.GetErrors());
                }
                return Created($"api/users/{result.Result}", default);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("capcha/get")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRandomCapcha()
        {
            try
            {
                var capcha = await Task.FromResult(CapchaGeneratorSimpleVersion.GetRandomCapcha());
                if (!string.IsNullOrWhiteSpace(capcha))
                {
                    return Ok(capcha);
                }
                throw new NotGeneratedCodeException("Не удалось сгенерировать капчу.");
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}