using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Handlers.RemoteOfferModels.Commands;
using Business.Handlers.RemoteOfferModels.Queries;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    ///     RemoteOfferModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteOfferModelsController : BaseApiController
    {
        /// <summary>
        ///     List RemoteOfferModels
        /// </summary>
        /// <remarks>RemoteOfferModels</remarks>
        /// <return>List RemoteOfferModels</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<IEnumerable<RemoteOfferModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getByProjectId")]
        public async Task<IActionResult> GetByProjectId(long projectId)
        {
            var result = await Mediator.Send(new GetRemoteOfferModelsByProjectIdQuery
            {
                ProjectId = projectId
            });
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     Add RemoteOfferModel.
        /// </summary>
        /// <param name="createRemoteOfferModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateRemoteOfferModelCommand createRemoteOfferModel)
        {
            var result = await Mediator.Send(createRemoteOfferModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     Update RemoteOfferModel.
        /// </summary>
        /// <param name="updateRemoteOfferModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRemoteOfferModelCommand updateRemoteOfferModel)
        {
            var result = await Mediator.Send(updateRemoteOfferModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     Delete RemoteOfferModel.
        /// </summary>
        /// <param name="deleteRemoteOfferModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteRemoteOfferModelCommand deleteRemoteOfferModel)
        {
            var result = await Mediator.Send(deleteRemoteOfferModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}