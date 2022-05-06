using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.BusinessLogic.Abstracts;

namespace SolaraNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifficationSenderController : BaseController
    {
        private readonly IIdentityService _identity;
        public NotifficationSenderController(IMapper mapper, ILogger<NotifficationSenderController> logger, IIdentityService identity) : base(mapper, logger)
        {
            _identity = identity;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StartNotifficationSender(CancellationToken cancellationToken)
        {
            await _identity.SendNotifications("Andeyteyker2012@yandex.ru", "Sakura2000)", cancellationToken);
            return Ok();
        }
    }
}