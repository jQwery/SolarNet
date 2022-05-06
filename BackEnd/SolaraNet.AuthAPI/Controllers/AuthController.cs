using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.AuthAPI.AuthApiExceptions;
using SolaraNet.AuthAPI.Models;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using LoginModel = SolaraNet.AuthAPI.Models.LoginModel;

namespace SolaraNet.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;

        public AuthController(IMapper mapper, ILogger<AuthController> logger, IUserService userService, IConfiguration configuration, IIdentityService identityService) : base(mapper, logger)
        {
            _userService = userService;
            _configuration = configuration;
            _identityService = identityService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel user, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _identityService.CreateToken(new BusinessLogic.Contracts.Models.LoginModel()
                    { Email = user.Email, Password = user.Password });
                if (!token.Success)
                {
                    throw new NotLoginException("Не удалось залогиниться. И вот почему: " + token.GetErrors());
                }

                if (token.Result == "banned")
                {
                    return BadRequest("Пользователь забанен");
                }
                return Ok(new
                {
                    access_token = token.Result
                }); // возвращаем токен
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}