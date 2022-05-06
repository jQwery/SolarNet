using Microsoft.AspNetCore.Mvc;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Isopoh.Cryptography.Argon2;
using System.Collections.Generic;
using System.Linq;
using SolaraNet;
using System.Threading.Tasks;
using SolaraNet.DataAccessLayer.EntityFramework.Repositories;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SolaraNet.ApiExceptions;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.Models.SpecialModels;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SolaraNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ISaver _saver;
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public UserController(IMapper mapper, ILogger logger, IConfiguration configuration, IUserService userService, ISaver saver, IEmailService emailService, IIdentityService identityService) : base(mapper, logger)
        {
            _configuration = configuration;
            _userService = userService;
            _saver = saver;
            _emailService = emailService;
            _identityService = identityService;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserById(int id, CancellationToken cancellationToken)
        {
            var result = await _userService.DeleteUserById(id, cancellationToken);
            if (!result.Success)
            {
                throw new NotDeleteUserException("Не получилось удалить пользователя. Подробности: " + result.GetErrors());
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserById(id, cancellationToken);
            if (!user.Success)
            {
                throw new NotFoundUserException("Не удалось получить пользователя. Подробности: " + user.GetErrors());
            }
            return Ok(user.Result);
        }

    }
}

