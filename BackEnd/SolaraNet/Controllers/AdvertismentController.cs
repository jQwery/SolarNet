using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.ApiExceptions;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.BusinessLogic.Implementations;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.Models;
using SolaraNet.Models.SpecialModels;
using SimplePaginationModel = SolaraNet.Models.SimplePaginationModel;

namespace SolaraNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertismentController:BaseController
    {
        private readonly IAdvertismentService _advertismentService;
        public AdvertismentController(IMapper mapper, ILogger<AdvertismentController> logger, IAdvertismentService advertismentService) : base(mapper, logger)
        {
            _advertismentService = advertismentService;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] GetRequestAdvertismentsModel request, CancellationToken cancellationToken)
        {
            try
            {
                #region Небольшая валидация
                decimal veryBigValue = 888888888;
                decimal verySmallValue = 0;
                if (request.MinimumCost == null)
                {
                    request.MinimumCost = verySmallValue;
                }
                if (request.MaximumCost == null)
                {
                    request.MaximumCost = veryBigValue;
                }
                #endregion
                var result = await OrderBy(request, cancellationToken);
              
                var task = OrderBy(request, cancellationToken);
                if (!result.Success)
                {
                    throw new NotGetAllAdvertismentsException("Не получилось получить по-нормальному объявления. Подробности: " + result.GetErrors());
                }
                List<AdvertismentForFront> advertismetnsToBeReturned = new();

                foreach (var dto in result.Result)
                {

                    var advertisment = _mapper.Map<AdvertismentForFront>(dto);
                    advertisment.Images = new List<string>();
                    foreach (var image in dto.ImagesList)
                    {

                        advertisment.Images.Add(image);
                    }
                    advertismetnsToBeReturned.Add(advertisment);
                }
                return Ok(advertismetnsToBeReturned);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdvertismentsCount([FromQuery] PageCountModel model, CancellationToken cancellationToken)
        {
            try
            {
                #region Небольшая валидация
                decimal veryBigValue = 888888888;
                decimal verySmallValue = 0;
                if (model.MinimumCost == null)
                {
                    model.MinimumCost = verySmallValue;
                }
                if (model.MaximumCost == null)
                {
                    model.MaximumCost = veryBigValue;
                }
                #endregion
                var result = await _advertismentService.GetPagesCount(model, cancellationToken);
                if (!result.Success)
                {
                    throw new NotGetAdvertismentCount("Не получилось получить количество объявлений. Подробности: " + result.GetErrors());
                }
                return Ok(result.Result);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("my/count")]
        [Authorize]
        public async Task<IActionResult> GetMyAdvertismentsCount([FromQuery] MyAdvertismentPaginationModel request, CancellationToken cancellationToken)
        {
            var result =
                await _advertismentService.GetMyPagesCount(_mapper.Map<MyPaginationModel>(request), cancellationToken);
            return Ok(result.Result);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewAdvertisment([FromBody] AdvertismentFromFront advertismentFromFront, CancellationToken cancellationToken)
        {
            try
            {
                var advertisment = _mapper.Map<AdvertismentDTO>(advertismentFromFront);
                advertisment.ImagesList = new List<string>();
                advertisment.ImagesList.AddRange(advertismentFromFront.Images);
                var result = await _advertismentService.CreateNewAdvertisment(advertisment, cancellationToken);
                if (!result.Success)
                {
                    throw new NotCreatedAdvertismentException("Не удалось создать объявление. Подробности: " + result.GetErrors());
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdvertisment(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.Delete(id, cancellationToken);
                if (!result.Success)
                {
                    throw new NotDeletedAdvertismentException("Не удалось удалить объявление. Подробности: " + result.GetErrors());
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> UpdateAdvertisment([FromBody] UpdateAdvertismentModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.Update(new AdvertismentDTO()
                {
                    AdvertismentID = model.AdvertismentId,
                    AdvertismentTitle = model.AdvertismentTitle,
                    Description = model.Description,
                    Price = model.Price,
                    Category = new CategoryDTO()
                    {
                        Id = model.CategoryId
                    },
                    City = model.City,
                    ImagesList = model.Images
                }, cancellationToken);

                if (!result.Success)
                {
                    throw new NotUpdatedAdvertismentException("Не удалось обновить объявление. Подробности: " + result.GetErrors());
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.GetCategories(cancellationToken);
                if (!result.Success)
                {
                    throw new NoCategoriesFoundExceptions("Не удалось найти категории. Подробности: " + result.GetErrors());
                }
                return Ok(result.Result);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Получить подкатегории какой-то категории по id, например, чтобы получить подкатегории категории "Танспортные средства", нужно указать запрос http:localhost:5050/api/undercategory/1, где 1 - это порядковый номер категории. Ну, малость запутанно, но у нас и не будет 100 категорий.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("undercategory/{categoryId}")]
        public async Task<IActionResult> GetUndercategodiesOfCategory(int categoryId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.GetUnderCategories(categoryId, cancellationToken);
                if (!result.Success)
                {
                    throw new NoCategoriesFoundExceptions("Не удалось найти ПОДкатегории. Подробности: " + result.GetErrors());
                }
                return Ok(result.Result);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Получить объявление по id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdvertismentById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.GetById(id, cancellationToken);
                if (!result.Success)
                {
                    throw new NotFoundAdvertismentException(result.GetErrors());
                }
                if (result.Result == null)
                {
                    return BadRequest("Объявления с таким Id нет");
                }
                List<AdvertismentForFront> advertismetnsToBeReturned = new();
                var advertisment = _mapper.Map<AdvertismentForFront>(result.Result);
                advertisment.Images = new List<string>();
                if (result.Result.ImagesList == null)
                {
                    result.Result.ImagesList = new List<string>();
                }
                foreach (var image in result.Result.ImagesList)
                {
                    advertisment.Images.Add(image);
                }
                advertismetnsToBeReturned.Add(advertisment);

                return Ok(advertisment);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetUserAdvertisments([FromQuery] MyAdvertismentPaginationModel request, CancellationToken cancellationToken)
        {
            try
            {
                var advertisments = await _advertismentService.GetPagedMyAdvertisments(_mapper.Map<MyPaginationModel>(request), cancellationToken);
                if (!advertisments.Success)
                {
                    return BadRequest(advertisments.GetErrors());
                }
                List<AdvertismentForFront> advertismentsToReturn = _mapper.Map<IList<AdvertismentForFront>>(advertisments.Result).ToList();
                for (int i = 0; i < advertisments.Result.Count(); i++)
                {
                    advertismentsToReturn[i].Images = new List<string>();
                    if (advertisments.Result[i].ImagesList != null)
                    {
                        advertismentsToReturn[i].Images.AddRange(advertisments.Result[i].ImagesList);
                    }
                }

                return Ok(advertismentsToReturn);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("byUserId")]
        public async Task<IActionResult> GetAdvertismentsByUserId([FromQuery] PaginationByUserId request,
            CancellationToken cancellationToken)
        {
            try
            {
                var advertisments = await _advertismentService.GetPagedAdvertismentsByUserId(_mapper.Map<PaginationByUserId>(request), cancellationToken);
                if (!advertisments.Success)
                {
                    _logger.LogError(advertisments.GetErrors());
                    return BadRequest(advertisments.GetErrors());
                }
                List<AdvertismentForFront> advertismentsToReturn = _mapper.Map<IList<AdvertismentForFront>>(advertisments.Result).ToList();
                for (int i = 0; i < advertisments.Result.Count(); i++)
                {
                    advertismentsToReturn[i].Images = new List<string>();
                    if (advertisments.Result[i].ImagesList != null)
                    {
                        advertismentsToReturn[i].Images.AddRange(advertisments.Result[i].ImagesList);
                    }
                }

                return Ok(advertismentsToReturn);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("byUserId/count")]
        public async Task<IActionResult> GetAdvertismentsByUserIdCount([FromQuery] PaginationByUserId request, CancellationToken cancellationToken)
        {
            var result = await _advertismentService.GetUserIdAdvertismentsPagesCount(request, cancellationToken);

            return Ok(result.Result);
        }

        [HttpGet("new-advertisments")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetNewAdvertisments([FromQuery] GetRequestAdvertismentsModel request, CancellationToken cancellationToken)
        {
            try
            {
                #region Небольшая валидация
                decimal veryBigValue = 888888888;
                decimal verySmallValue = 0;
                if (request.MinimumCost == null)
                {
                    request.MinimumCost = verySmallValue;
                }
                if (request.MaximumCost == null)
                {
                    request.MaximumCost = veryBigValue;
                }
                #endregion
                var result = await OrderBy(request, cancellationToken, EntityStatus.Created);
                if (!result.Success)
                {
                    throw new NotGetAllAdvertismentsException("Не получилось получить по-нормальному объявления. Подробности: " + result.GetErrors());
                }
                List<AdvertismentForFront> advertismetnsToBeReturned = new();
                foreach (var dto in result.Result)
                {
                    var advertisment = _mapper.Map<AdvertismentForFront>(dto);
                    advertisment.Images = new List<string>();
                    foreach (var image in dto.ImagesList)
                    {
                        advertisment.Images.Add(image);
                    }
                    advertismetnsToBeReturned.Add(advertisment);
                }

                return Ok(advertismetnsToBeReturned);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveAdvertisment(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.ApproveAdvertisment(id, cancellationToken);
                if (!result.Success)
                {
                    throw new NotApprovedException(result.GetErrors());
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectAdvertisment([FromBody] RejectionModelApi request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _advertismentService.RejectAdvertisment(_mapper.Map<RejectionModel>(request), cancellationToken);
                if (!result.Success)
                {
                    throw new NotRejectedAdvertismentException(result.GetErrors());
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("expensive")]
        public async Task<IActionResult> GetExpensiveAdvertismentCost([FromQuery] ExpensiveAdvertismentCostModel request,
            CancellationToken cancellationToken)
        {
            try
            {
                decimal minimumCost = 0;
                decimal maximumCost = 888888888;
                int limit = Int32.MaxValue;
                int offset = 0;
                OperationResult<decimal> result = default;
                if (request.IdCategory != null)
                {
                    result = await _advertismentService.GetTheMostExpensiveAdvertismentCost(new PageViewModel(
                        new CategoryDTO()
                        {
                            Id = (int)request.IdCategory
                        }, limit, offset, request.Date, minimumCost, maximumCost,
                        request.OnlyWithPhoto, request.OnlyWithComments, request.ByDescendingCost,
                        request.KeyWords, request.City, true, false), cancellationToken);
                }
                else
                {
                    result = await _advertismentService.GetTheMostExpensiveAdvertismentCost(new PageViewModel(null, limit, offset, request.Date, minimumCost, maximumCost,
                        request.OnlyWithPhoto, request.OnlyWithComments, request.ByDescendingCost,
                        request.KeyWords, request.City, true, false), cancellationToken);
                }
                if (!result.Success)
                {
                    throw new NotGetAllAdvertismentsException("Не получилось получить по-нормальному объявления. Подробности: " + result.GetErrors());
                }

                return Ok(new { cost = result.Result });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        private async Task<OperationResult<IList<AdvertismentDTO>>> OrderBy(GetRequestAdvertismentsModel request, CancellationToken cancellationToken, EntityStatus status = EntityStatus.Active)
        {
            OperationResult<IList<AdvertismentDTO>> result = default;
            if (request.IdCategory!=null)
            {
               result = await _advertismentService.GetPaged(new PageViewModel(new CategoryDTO()
                {
                    Id = (int) request.IdCategory
                }, request.Limit, request.Offset, request.Date, (decimal) request.MinimumCost, (decimal) request.MaximumCost, request.OnlyWithPhoto, request.OnlyWithComments,  request.ByDescending, request.KeyWords, request.City, request.SortByDate, request.SortByCost, status), cancellationToken);
            }
            else
            {
                result = await _advertismentService.GetPaged(new PageViewModel(null, request.Limit, request.Offset, request.Date, (decimal) request.MinimumCost, (decimal) request.MaximumCost, request.OnlyWithPhoto, request.OnlyWithComments,  request.ByDescending, request.KeyWords, request.City, request.SortByDate, request.SortByCost, status), cancellationToken);
            }

            return OperationResult<IList<AdvertismentDTO>>.Ok(result?.Result);


        }
    }
}