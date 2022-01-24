using System.Threading.Tasks;
using Business.Handlers.InterstitialAdEventModels.Commands;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    ///     InterstitialAdEventModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InterstitialAdEventModelsController : BaseApiController
    {
        /// <summary>
        ///     Add InterstitialAdEventModel.
        /// </summary>
        /// <param name="createInterstitialAdEventModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost]
        public async Task<IActionResult> Add(
            [FromBody] CreateInterstitialAdEventModelCommand createInterstitialAdEventModel)
        {
            var result = await Mediator.Send(createInterstitialAdEventModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}