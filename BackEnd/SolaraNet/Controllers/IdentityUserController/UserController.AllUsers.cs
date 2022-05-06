using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Mapper;
using SolaraNet.Models;

namespace SolaraNet.Controllers.IdentityUserController
{
    public partial class UserController
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers([FromQuery] SimplePaginationModel request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetUsers(_mapper.Map<SimplePagination>(request), cancellationToken);
            List<User> users = new();
            users = _mapper.Map<List<User>>(result.Result);
            return Ok(users);
        }

        [HttpGet("all/countPages")]
        public async Task<IActionResult> GetPagesCount([FromQuery] SimplePaginationModel request, CancellationToken cancellationToken)
        {
            var result =
                await _identityService.GetUsersPagesCount(_mapper.Map<SimplePagination>(request), cancellationToken);

            return Ok(result.Result);
        }
    }
}