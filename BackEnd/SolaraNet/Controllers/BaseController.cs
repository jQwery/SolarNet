using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.BusinessLogic.Contracts;

namespace SolaraNet.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        public BaseController(
            IMapper mapper, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;
        }
        protected IActionResult ProcessOperationResult<T>(OperationResult<T> operationResult)
        {
            if (!operationResult.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, operationResult.GetErrors());
            }
            return Ok(operationResult.Result);
        }

        protected async Task<IActionResult> ProcessOperationResult<T>(Func<Task<OperationResult<T>>> action)
        {
            try
            {
                var operationResult = await action.Invoke();
                if (!operationResult.Success)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, operationResult.GetErrors());
                }
                return Ok(operationResult.Result);
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Произошла непредвиденная ошибка обратитесь в службу поддержки. Подробности: " + e);
            }
        }


        protected IActionResult ProcessOperationResult<T>(Func<OperationResult<T>> action)
        {
            try
            {
                var operationResult = action.Invoke();
                if (!operationResult.Success)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, operationResult.GetErrors());
                }
                return Ok(operationResult.Result);
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Произошла непредвиденная ошибка обратитесь в службу поддержки. Подробности: " + e);
            }
        }
    }
}