using System.Net;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly IMediator _mediator = null;
        protected IMediator Mediator => _mediator ?? HttpContext.RequestServices.GetService<IMediator>();

        protected IActionResult HandleResult<T>(Response<T> response)
        {
            switch (response.Result)
            {
                case ResponseResult.BadRequestStructure:
                    return BadRequest(response.ErrorMessage);
                case ResponseResult.UserIsNotAuthorized:
                    return Unauthorized(response.ErrorMessage);
                case ResponseResult.AccessDenied:
                    return StatusCode((int) HttpStatusCode.Forbidden, response.ErrorMessage);
                case ResponseResult.ResourceDoesntExist:
                    return NotFound(response.ErrorMessage);
                case ResponseResult.InternalError:
                    return StatusCode((int) HttpStatusCode.InternalServerError, response.ErrorMessage);
                case ResponseResult.DataObtained:
                    return Ok(response.Value);
                case ResponseResult.Created: 
                case ResponseResult.Deleted: 
                case ResponseResult.Updated:
                    return NoContent();
                default:
                    return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}