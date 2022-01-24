using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Handlers.RemoteOfferHistoryModels.Commands;
using Business.Handlers.RemoteOfferHistoryModels.Queries;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    ///     RemoteOfferHistoryModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteOfferHistoryModelsController : BaseApiController
    {
        /// <summary>
        ///     List RemoteOfferModels
        /// </summary>
        /// <remarks>RemoteOfferModels</remarks>
        /// <return>List RemoteOfferModels</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IDataResult<IEnumerable<RemoteOfferHistoryModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getByProjectId")]
        public async Task<IActionResult> GetByProjectId(string projectId)
        {
            var result = await Mediator.Send(new GetOfferHistoryModelsByProjectIdQuery
            {
                ProjectId = projectId
            });
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     Add RemoteOfferHistoryModel.
        /// </summary>
        /// <param name="createRemoteOfferHistoryModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost]
        public async Task<IActionResult> Add(
            [FromBody] CreateRemoteOfferHistoryModelCommand createRemoteOfferHistoryModel)
        {
            var result = await Mediator.Send(createRemoteOfferHistoryModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}