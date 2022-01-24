using System.Threading.Tasks;
using Business.Handlers.RemoteOfferEventModels.Commands;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    ///     RemoteOfferEventModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteOfferEventModelsController : BaseApiController
    {
        /// <summary>
        ///     Add RemoteOfferEventModel.
        /// </summary>
        /// <param name="createRemoteOfferEventModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateRemoteOfferEventModelCommand createRemoteOfferEventModel)
        {
            var result = await Mediator.Send(createRemoteOfferEventModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}