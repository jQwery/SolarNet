using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.ApiExceptions;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models;

namespace SolaraNet.Controllers.IdentityUserController
{
    public partial class UserController
    {
        [HttpPost("change-data")]
        [Authorize]
        public async Task<IActionResult> ChangeUserData([FromBody] ChangeUserDataModel request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(request.NewEmail))
                {
                    var resultEmail = await _userService.UpdateUserEmail(new ChangeEmailModel()
                    {
                        NewEmail = request.NewEmail
                    }, cancellationToken); // mapper
                    if (!resultEmail.Success)
                    {
                        throw new NotUpdatedUserEmailException("Не удалось изменить Email, подробности: " +
                                                               resultEmail.GetErrors());
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.NewPassword))
                {
                    var resultPassword = await _userService.ChangePassword(new ChangePasswordModel()
                    {
                        CurrentPassword = request.CurrentPassword,
                        NewPassword = request.NewPassword
                    }, cancellationToken);
                    if (!resultPassword.Success)
                    {
                        throw new NotUpdatedUserPasswordException("Не удалось изменить пароль пользователя. Подробности: " +
                                                                  resultPassword.GetErrors());
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Phone))
                {
                    var resultPhone = await _userService.ChangeUserMobilePhoneNumber(request.Phone, cancellationToken);
                    if (!resultPhone.Success)
                    {
                        throw new NotChangedPhoneNumberException(resultPhone.GetErrors());
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    var resultName = await _userService.ChangeUserName(request.Name, cancellationToken);
                    if (!resultName.Success)
                    {
                        throw new NotChangedUserNameException(resultName.GetErrors());
                    }
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e);
            }
        }
    }
}