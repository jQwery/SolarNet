using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaraNet.ApiExceptions;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models.SpecialModels;

namespace SolaraNet.Controllers
{
    // эта часть класса отвечает за смену пароля и логина
    public partial class UserController
    {
        /// <summary>
        /// Получаем Id пользователя
        /// </summary>
        private int _userId => Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Email).Value);
        /// <summary>
        /// Получаем мыло пользователя
        /// </summary>
        private string _email => User.Claims.Single(e => e.Type == ClaimTypes.Email).Value;

        /// <summary>
        /// Отссылаем на почту код подтверждения
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(CancellationToken cancellationToken)
        {
            var user = await ValidUser(cancellationToken); // Маловероятно, что это когда - нибудь всерьёз пригодится, но если что предотвратит хакерскую атаку
            var code = await _emailService.GenerateCode(cancellationToken);
            if (!code.Success)
            {
                throw new NotGeneratedCodeException("Не получилось сгенирировать код, подробности: " + code.GetErrors());
            }
            _codes.Add(_email, new TemporaryUser() { Code = code.Result.ToString(), Password = null });
            var result = await _emailService.SendEmailAsync(_email, "Reset password", GenerateCoolMessageAboutChangingPassword(code.Result.ToString()), "eduard.pirogov.2000@gmail.com", "какой-то пароль", cancellationToken);
            if (!result.Success)
            {
                throw new NotSendEmailException("Не удалось отослать мыло. Подробности: " + result.GetErrors());
            }
            return Ok();
        }

        /// <summary>
        /// Принимаем пароль
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> ChangePassword(ResetPasswordModel model, CancellationToken cancellationToken)
        //{
        //    var user = await ValidUser(cancellationToken);
        //    ValidateCode(_email, model.Code); // валидируем код
        //    var newPasswordHash = Argon2.Hash(model.NewPassword);
        //    ValidPassword(newPasswordHash, user.Password);
        //    var result = await _userService.UpdateUserPassword(user.Id, newPasswordHash, cancellationToken); // меняем пароль
        //    if (!result.Success)
        //    {
        //        throw new NotUpdatedUserPasswordException($"Пароль пользователя с id: {user.Id} не удалось сменить. Подробности: " + result.GetErrors());
        //    }
        //    var nextResult = await _userService.UpdateUserEmail(user.Id, model.NewEmail, cancellationToken); // меняем мыло
        //    if (!nextResult.Success)
        //    {
        //        throw new NotUpdatedUserEmailException($"Не удалось сменить мыло пользователя с id {user.Id}. Подробности: " + result.GetErrors());
        //    }
        //    return Ok();
        //}

        /// <summary>
        /// Валидация юзера. В случае успеха возвращается UserDTO
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<UserDTO> ValidUser(CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserById(_userId, cancellationToken);
            if (user.Result == null)
            {
                throw new NotFoundUserException("Юзера с таким email не существует");
            }
            return user.Result;
        }
        /// <summary>
        /// Метод, который возвращает HTML письмо с кодом подтверждения для смены пароля. Параметром принимает то, что нужно доставить пользователю, например, код подтверждения
        /// </summary>
        /// <param name="toBeDelivered"></param>
        /// <returns></returns>
        private string GenerateCoolMessageAboutChangingPassword(string toBeDelivered) => $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Подтверждение смены пароля на сайте SolaraNet</title>\r\n</head>\r\n<body>\r\n    <h1 style=\"text-align: center;\">Подтверждение смены пароля на сайте SolaraNet</h1>\r\n    <p>Ваш код подтверждения:{toBeDelivered} </p>\r\n    <br><br><br>\r\n    <p style=\"color: red\">Если вы получили это письмо, просто проигнорируйте его, а ещё лучше удалите, потому что кто-то пытается сменить пароль от Вашей учётной записи.</p>\r\n</body>\r\n</html>";

        private void ValidPassword(string oldPassword, string newPassword)
        {
            if (newPassword == oldPassword)
            {
                throw new IncorrectPasswordException("Новый пароль похож на старый");
            }
        }
    }
}