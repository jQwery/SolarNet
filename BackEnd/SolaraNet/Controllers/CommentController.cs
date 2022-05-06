using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.ApiExceptions;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models;
using SolaraNet.Models.SpecialModels;

namespace SolaraNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        private readonly IAdvertismentService _advertismentService;

        public CommentController(IMapper mapper, ILogger<CommentController> logger, IAdvertismentService advertismentService) : base(mapper, logger)
        {
            _advertismentService = advertismentService;
        }

        /// <summary>
        /// Получить комментарии конкретного объявления
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("comments-of-advertisment")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] GetRequestCommentModel request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.GetPagedComments(new CommentPageViewModel(request.AdvertismentId, request.Limit, request.Offset), cancellationToken);
                return Ok(result.Result);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await _advertismentService.DeleteComment(id, cancellationToken);
                if (!deleteResult.Success)
                {
                    throw new NotDeletedCommentException(deleteResult.GetErrors());
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsCount([FromQuery] GetRequestCommentModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.GetPagesOfCommentsCount(
                    new CommentPageViewModel(model.AdvertismentId, model.Limit, model.Offset), cancellationToken); 
                return Ok(result.Result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("add-comment")]
        [Authorize]
        public async Task<IActionResult> AddCommentToAdvertisment([FromBody] AddCommentModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.AddComment(model.AdvertismentId, new CommentDTO()
                {
                    CommentText = model.Text
                }, cancellationToken);
                if (!result.Success)
                {
                    throw new NotCreatedCommentException(result.GetErrors());
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}