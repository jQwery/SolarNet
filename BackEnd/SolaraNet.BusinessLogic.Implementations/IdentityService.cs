using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AbstractsExceptions;
using AutoMapper;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.Common.Validators;

namespace SolaraNet.BusinessLogic.Implementations
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<DBUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _mailService;
        private readonly IMapper _mapper;
        public IdentityService(IHttpContextAccessor httpContextAccessor, UserManager<DBUser> userManager, IConfiguration configuration, IEmailService mailService, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _mapper = mapper;
        }

        public async Task<OperationResult<string>> GetCurrentUserId(CancellationToken cancellationToken = default)
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            if (claimsPrincipal == null)
            {
                return OperationResult<string>.Failed(new []{"claimPrincipal равен null, то месть в _httpContext'e почему не валидный токен лежит, либо вообще ничего не лежит."});
            }
            return OperationResult<string>.Ok(await Task.FromResult(_userManager.GetUserId(claimsPrincipal)));
        }

        public async Task<OperationResult<UserDTO>> GetUserById(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user==null)
            {
                return OperationResult<UserDTO>.Failed(new []{"Не удалось найти пользователя по Id. Ошибка в IdentityService, метод GetUserById, строка _userManager.FindByIdAsync(id)"});
            }
            var mappedUser = _mapper.Map<UserDTO>(user);

            return OperationResult<UserDTO>.Ok(mappedUser);
        }

        public async Task<OperationResult<bool>> IsInRole(string userId, string role, CancellationToken cancellationToken)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null)
            {
                return OperationResult<bool>.Failed(new[] {"Пользователь не найден"});
            }

            return OperationResult<bool>.Ok(await _userManager.IsInRoleAsync(identityUser, role));
        }

        public async Task<OperationResult<string>> CreateUser(UserDTO request, CancellationToken cancellationToken = default)
        {
            var existedUser = await _userManager.FindByEmailAsync(request.Mail);
            if (existedUser != null)
            {
                return OperationResult<string>.Failed(new []{"Пользователь с такой почтой уже существует"});
            }
            var identityUser = new DBUser()
            {
                UserName = request.Name,
                Email = request.Mail,
                PhoneNumber = request.PhoneNumber,
                Avatar = new DBAvatar()
            };
            var identityResult = await _userManager.CreateAsync(identityUser, request.Password);
            if (!identityResult.Succeeded)
            {
                return OperationResult<string>.Failed(new []{"Не удалось создать пользователя на стороне Identity, то есть метод _userManager.CreateAsync() завершился с ошибОчкой. Подробности: " + identityResult.Errors});
            }
            await _userManager.AddToRoleAsync(identityUser, request.Role); // добавили роль пользователю
            var result = await SendConfirmEmail(identityUser, request, "Andeyteyker2012@yandex.ru", "Sakura2000)",
                cancellationToken);
            if (!result.Success)
            {
                await _userManager.DeleteAsync(identityUser); // удаляем этого пользователя, так как с ним что-то явно пошло не так.
                return OperationResult<string>.Failed(new []{result.GetErrors()});
            }

            return OperationResult<string>.Ok(identityUser.Id); // создать пользователя удалось
        }

        public async Task<OperationResult<bool>> ChangeUserPassword(ChangePasswordModel model, CancellationToken cancellationToken)
        {
            var userId = await GetCurrentUserId(cancellationToken);
            if (!userId.Success)
            {
                return OperationResult<bool>.Failed(new []{"Метод GetCurrentUserId завершился с ошибкой. Подробности: " + userId.GetErrors()});
            }
            var identityUser = await _userManager.FindByIdAsync(userId.Result); // ищем identityUser'a
            if (identityUser==null)
            {
                return OperationResult<bool>.Failed(new []{"Не удалось найти пользователя. То есть в методе ChangeUserPassword, а именно в той части где _userManager.FindByAsync произошёл сбой, этот метод вернул null."});
            }
            var result = await _userManager.ChangePasswordAsync(identityUser, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return OperationResult<bool>.Failed(new []{"Не удалось сменить пароль. Метод _userManager.ChangePasswordAsync вернул плохой результат. Ну, сам почитай: "+result.Errors});
            }
            return OperationResult<bool>.Ok(true); // удалось сменить пароль, всё хорошо
        }

        public async Task<OperationResult<bool>> ChangeUserEmail(ChangeEmailModel model, CancellationToken cancellationToken)
        {
            var userId = await GetCurrentUserId(cancellationToken);
            var user = await _userManager.FindByIdAsync(userId.Result);
            if (user==null)
            {
                return OperationResult<bool>.Failed(new[] { "Не удалось найти пользователя. То есть в методе ChangeUserEmail, а именно в той части где _userManager.FindByAsync произошёл сбой, этот метод вернул null." });
            }
            if (user.Email == model.NewEmail)
            {
                return OperationResult<bool>.Ok(true);
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail); // можно было бы это прислать по почте, но это имеет свои недостатки: почту просто так не меняют. Чаще всего так делают, когда доступа к прошлой почте больше нет...
            var validationResult = await Task.Run((() => StringValidator.CheckString(token)));
            if (!validationResult)
            {
                return OperationResult<bool>.Failed(new []{"Странная вещь произошла, почему-то генерируется странный токен в методе ChangeUserEmail."});
            }
            var result = await _userManager.ChangeEmailAsync(user, model.NewEmail, token); // меняем email
            if (!result.Succeeded)
            {
                return OperationResult<bool>.Failed(new []{"Не удалось изменить email. Ошибка в методе _userManager.ChangeEmailAsync. Подробности: "+result.Errors});
            }

            user.EmailConfirmed = false; // так как меняем почту, то нужно снова её подтвердить
            var resultSending = await SendConfirmEmail(user, new UserDTO(){Mail = model.NewEmail}, "Andeyteyker2012@yandex.ru", "Sakura2000)",
                cancellationToken);
            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> SendConfirmEmail(DBUser identityUser, UserDTO request, string adminEmail, string passwordOfEmail, CancellationToken cancellationToken = default)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var encodedToken = HttpUtility.UrlEncode(token); 
            
            var result = await _mailService.SendEmailAsync(request.Mail, "Подтверждение почты",
                GenerateCoolMessageAboutRegistration(identityUser.Id, encodedToken), adminEmail, passwordOfEmail, cancellationToken);
            if (!result.Success)
            {
                return OperationResult<bool>.Failed(new []{result.GetErrors()});
            }
            return OperationResult<bool>.Ok(true); // письмо было отправлено
        }

        public async Task<OperationResult<string>> CreateToken(LoginModel request, CancellationToken cancellationToken = default)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Email);
            if (identityUser == null)
            {
                return OperationResult<string>.Failed(new[] { "Не удалось найти пользователя на стороне Identity, то есть метод _userManager.FindByEmailAsync() завершился с ошибОчкой, вернее вернул null." });
            }

            if (identityUser.Status==EntityStatus.Deleted)
            {
                return OperationResult<string>.Ok("banned");
            }
            var passwordCheckResult = await _userManager.CheckPasswordAsync(identityUser, request.Password);
            if (!passwordCheckResult)
            {
                return OperationResult<string>.Failed(new []{"Неверный логин или пароль. Ошибка на стороне Identity, то есть метод _userManager.CheckPasswordAsync() вернул false"});
            }
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(identityUser);
            if (!isEmailConfirmed)
            {
                return OperationResult<string>.Failed(new []{"Почта не подтверждена. Метод _userManager.IsEmailConfirmedAsync() вернул false"});
            }

            List<Claim> claims;
            claims = new List<Claim>
            {
                new(ClaimTypes.Email, request.Email),
                new(ClaimTypes.NameIdentifier, identityUser.Id),
                new(ClaimTypes.Name, identityUser.UserName),
                new(ClaimTypes.MobilePhone, identityUser.PhoneNumber ?? String.Empty),
                new(ClaimTypes.UserData, identityUser.Avatar?.ImageLink ?? "https://cs.pikabu.ru/post_img/big/2013/08/24/1/1377296637_1500370441.png")
            };
            var userRoles = await _userManager.GetRolesAsync(identityUser);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"])),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return OperationResult<string>.Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<OperationResult<bool>> ConfirmEmail(string userId, string token, CancellationToken cancellationToken = default)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null)
            {
                return OperationResult<bool>.Failed(new[]
                    {"Пользователь не найден, метод _userManager.FindByAsync() ничего не нашёл."});
            }

            var result = await _userManager.ConfirmEmailAsync(identityUser, token);
            if (!result.Succeeded)
            {
                return OperationResult<bool>.Failed(new []{"Не удалось подтвердить мыло. Скорее всего, невалидный токен. А что уж там невалидно и почему ответит breakpoint."});
            }
            return OperationResult<bool>.Ok(result.Succeeded);
        }

        public async Task<OperationResult<bool>> ChangeAvatar(string newAvatar, CancellationToken cancellationToken)
        {
            var id = await GetCurrentUserId(cancellationToken);
            if (!id.Success)
            {
                return OperationResult<bool>.Failed(new []{id.GetErrors()});
            }
            var user = await _userManager.FindByIdAsync(id.Result);
            if (user.Avatar==null)
            {
                user.Avatar = new DBAvatar();
            }
            user.Avatar.ImageLink = newAvatar;
            await _userManager.UpdateAsync(user);

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> ChangeMobilePhoneNumber(string newNumber, CancellationToken cancellationToken)
        {
            var id = await GetCurrentUserId(cancellationToken);
            if (!id.Success)
            {
                return OperationResult<bool>.Failed(new[] { id.GetErrors() });
            }
            var user = await _userManager.FindByIdAsync(id.Result);
            if (user==null)
            {
                return OperationResult<bool>.Failed(new []{"Не удалось найти пользователя с таким Id"});
            }
            var validationToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, newNumber);
            if (string.IsNullOrWhiteSpace(validationToken))
            {
                return OperationResult<bool>.Failed(new []{"Не удалось получить токен для смены номера телефона"});
            }
            var changeResult = await _userManager.ChangePhoneNumberAsync(user, newNumber, validationToken);
            if (!changeResult.Succeeded)
            {
                return OperationResult<bool>.Failed(new []{"Не удалось изменить номер телефона, ошибка в методе _userManager.ChangePhoneNumberAsync"});
            }
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return OperationResult<bool>.Failed(new []{"Не удалось обновить данные пользователя, ошибка в методе _userManager.UpdateAsync"});
            }

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> ChangeUserName(string name, CancellationToken cancellationToken)
        {
            var id = await GetCurrentUserId(cancellationToken);
            if (!id.Success)
            {
                return OperationResult<bool>.Failed(new[] { id.GetErrors() });
            }
            var user = await _userManager.FindByIdAsync(id.Result);
            if (user == null)
            {
                return OperationResult<bool>.Failed(new[] { "Не удалось найти пользователя с таким Id" });
            }
            user.UserName = name;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return OperationResult<bool>.Failed(new[] { "Не удалось обновить данные пользователя, ошибка в методе _userManager.UpdateAsync" });
            }

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> BanUser(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user!=null)
            {
                user.Status = EntityStatus.Deleted;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return OperationResult<bool>.Failed(new[] { "Не удалось обновить данные пользователя, ошибка в методе _userManager.UpdateAsync" });
                }
                return OperationResult<bool>.Ok(true);
            }
            else
            {
                return OperationResult<bool>.Failed(new []{"Пользователя с таким Id нет. Нельзя забанить того, кого нет в принципе!"});
            }
        }

        public async Task<OperationResult<List<UserDTO>>> GetUsers(SimplePagination model, CancellationToken cancellationToken)
        {
            List<DBUser> users = await Task.FromResult(_userManager.Users.Where(x=>x.Status==EntityStatus.Active||x.Status==EntityStatus.Created).OrderBy(x => x.Id).Skip(model.Offset)
                .Take(model.Limit).ToList());
            List<UserDTO> result = new List<UserDTO>();
            result = _mapper.Map<List<UserDTO>>(users);
            foreach (var user in users.Select((x,i)=>new {Index = i, Value = x}))
            {
                var role = await _userManager.GetRolesAsync(user.Value);
                result[user.Index].Role = role.FirstOrDefault();
            }
            return OperationResult<List<UserDTO>>.Ok(result);
        }

        public async Task<OperationResult<int>> GetUsersPagesCount(SimplePagination model,
            CancellationToken cancellationToken)
        {
            var usersCount = _userManager.Users.Count();
            var pagesCount = await Task.FromResult<int>(GetPageCount(usersCount, model.Limit));

            return OperationResult<int>.Ok(pagesCount);
        }

        public async Task<OperationResult<bool>> SendNotifications(string adminEmail, string passwordOfEmail, CancellationToken cancellationToken)
        {
            var users = await GetUsers(new SimplePagination()
            {
                Limit = int.MaxValue,
                Offset = 0
            }, cancellationToken);
            foreach (var user in users.Result)
            {
                var result = await _mailService.SendEmailAsync(user.Mail, "Подтверждение почты",
                    GenerateNotifficationMessage(user.Name), adminEmail, passwordOfEmail, cancellationToken);
            }

            return OperationResult<bool>.Ok(true);
        }
        private int GetPageCount(int count, int perPage)
        {
            float result = (float)count / perPage;
            var truncatedValue = Math.Truncate(result); // округлили число
            var faction = result - truncatedValue; // получили дробную часть
            int finalResul = 0;
            if (faction > 0)
            {
                finalResul = (int)result + 1;
            }
            else
            {
                finalResul = (int)result;
            }
            return finalResul;
        }

        /// <summary>
        /// Метод, который возвращает HTML письмо с подтверждением регистрации. Параметром принимает то, что нужно доставить пользователю, например, код подтверждения
        /// </summary>
        /// <param name="toBeDelivered"></param>
        /// <param name="userId"></param>
        /// <param name="encodedToken"></param>
        /// <returns></returns>
        private string GenerateCoolMessageAboutRegistration(string userId, string encodedToken)
        {
            var h1 = HtmlElement.Create("h1").AddChildFluent("span", "Подтверждение регистрации на сайте SolarNet")
                .Build();
            var message = HtmlElement.Create("article").AddChildFluent("p", "Вы получили это письмо постольку, поскольку Вы (или в противном случае кто-то) пытаетесь зарегистрироваться на лучшей доске объявлений SolarNet. Если Вы хотите завершить процесс регистрации, нажмите на ссылку снизу (будет сложно её пропустить).").Build();
            var url =
                $"<a href={_configuration["ApiUri"]}api/user/confirm?userId={userId}&token={encodedToken}>Нажми меня, чтобы завершить процесс регистрации.</a>"; // href помешал использовать билдер)
            var warning = HtmlElement.Create("section").AddChildFluent("p",
                "Если Вы получили это письмо, но не пытались зарегистрироваться на SolarNet, то проигнорируйте его, так как кто-то пытается при помощи Вашей почты произвести процесс регистрации.");
            var result = new StringBuilder();
            result.Append(h1).Append(message).Append(url).Append(warning); // конфигурируем письмо

            return result.ToString();
        }

        private string GenerateNotifficationMessage(string userName)
        {
            var h1 = HtmlElement.Create("h1").AddChildFluent("span", $"{userName}, заходите к нам на SolaraNet");
            var message = HtmlElement.Create("article").AddChildFluent("p","Знали ли Вы, что ежедневно на нашем сервисе появляется более 1000 новых объявлений. Не исключено, что именно сегодня найдёте там то, что так давно искали. Более того, Вы можете выставить что-то на продажу, найти покупателя и получить внепланавую прибыль. Неплохо, не так ли? <br> Можете даже самого/саму себя выставить на доску объявлений, например, 'Программист Эдуард, ищу работу .Net разработчиком, желательно в компании Solarlab.' Перейти на сайт можно по ссылке ниже, но рекомендуем запомнить ссылку на сайт, она не такая уж и длинная: solaranet.ru");
            var url = $"<a href={_configuration["ApiUri"]}/swagger/index.html>Перейти на сайт</a>";
            var warning = HtmlElement.Create("section").AddChildFluent("p",
                "Вы не можете отказаться от рассылки, потому что мы не бросаем друзей. Теперь мы связаны на всю жизнь!");
            var result = new StringBuilder();
            result.Append(h1).Append(message).Append(url).Append(warning);

            return result.ToString();
        }

    }
}
