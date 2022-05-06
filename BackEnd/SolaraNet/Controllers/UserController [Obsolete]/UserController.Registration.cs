using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Mvc;
using SolaraNet.ApiExceptions;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models;
using SolaraNet.Models.SpecialModels;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SolaraNet.Controllers
{
    [Obsolete("Уже не актуально, используются устаревшие сервисы. Тут нет Identity.")]
    public partial class UserController
    {
        /// <summary>
        /// Нужно где-то как-то хранить коды подтверждений. Это словарь, где ключ - имя будущего юзера, а значение - его код и пароль
        /// </summary>
        private static IDictionary<string, TemporaryUser> _codes = new Dictionary<string, TemporaryUser>();

        [HttpPost("registration")]
        public async Task<IActionResult> RegistrateNewUser([FromBody]RegisterUser publicUser, CancellationToken cancellationToken)
        {
            var code = await _emailService.GenerateCode(cancellationToken);
            if (!code.Success)
            {
                throw new NotGeneratedCodeException("Не получилось сгенирировать код, подробности: " + code.GetErrors());
            }
            var result = await _emailService.SendEmailAsync(publicUser.Email, "Email verification", GenerateCoolMessageAboutRegistration(code.Result.ToString()), "Andeyteyker2012@yandex.ru", "****", cancellationToken);
            if (!result.Success)
            {
                throw new NotSendEmailException("Не удалось отправить сообщение. Подробности: " + result.GetErrors());
            }
            _codes.Add(publicUser.Email, new TemporaryUser(){Code = code.Result.ToString(), Password = Argon2.Hash(publicUser.Password)});
            return Ok(); 
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmRegistration([FromBody]ConfirmationModel model, CancellationToken cancellationToken)
        {
            ValidateCode(model.UserName, model.Code); // проверяем, валиден ли код
            var result = await _userService.AddNewUser(new UserDTO() {Mail = model.Code, Password = _codes[model.UserName].Password},
                cancellationToken);
            if (!result.Success)
            {
                throw new NotAddedNewUser("Не удалось добавить нового пользователя. Подробности: " + result.GetErrors());
            }
            _codes.Remove(model.UserName); // тут мы не учитываем, что человек может и не ввести код подтверждения, захломив таким образом память сервера
            return Ok();
        }

        /// <summary>
        /// Метод, который возвращает HTML письмо с подтверждением регистрации. Параметром принимает то, что нужно доставить пользователю, например, код подтверждения
        /// </summary>
        /// <param name="toBeDelivered"></param>
        /// <returns></returns>
        private string GenerateCoolMessageAboutRegistration(string toBeDelivered) => $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Подтверждение регистрации сайта SolaraNet</title>\r\n</head>\r\n<body>\r\n    <h1 style=\"text-align: center;\">Подтверждение регистрации сайта SolaraNet</h1>\r\n    <p>Ваш код подтверждения:{toBeDelivered} </p>\r\n    <br><br><br>\r\n    <p style=\"color: red\">Если вы получили это письмо, просто проигнорируйте его, а ещё лучше удалите, потому что кто-то пытается зарегистрировать свою учётную запись, используя Вашу электронную почту</p>\r\n</body>\r\n</html>";

        private void ValidateCode(string userName, string code)
        {
            if (_codes[userName].Code != code)
            {
                throw new IncorrectCodeException("Код подтверждения регистрации неверный");
            }
        }
        private sealed class TemporaryUser
        {
            public string? Password { get; set; } // не во всех случаях нужен
            public string Code { get; set; }
        }
    }
}