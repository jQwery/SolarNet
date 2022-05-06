using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.ApiExceptions;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.Models;

namespace SolaraNet.Controllers.IdentityUserController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public partial class UserController : BaseController
    {
        #region Сервисы и прочее
        private readonly IIdentityService _identityService;
        private readonly IIdentityUserService _userService;
        #endregion

        public UserController(IMapper mapper, ILogger<UserController> logger, IIdentityService identityService, IIdentityUserService userService) : base(mapper, logger)
        {
            _identityService = identityService;
            _userService = userService;
        }

    }
}