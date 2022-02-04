using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Handlers.InterstitialAdModels.Commands;
using Business.Handlers.InterstitialAdModels.Queries;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    ///     InterstielAdModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InterstielAdModelsController : BaseApiController
    {
        /// <summary>
        ///     List InterstielAdModels
        /// </summary>
        /// <remarks>InterstielAdModels</remarks>
        /// <return>List InterstielAdModels</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<IEnumerable<InterstitialAdModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getByProjectId")]
        public async Task<IActionResult> GetByProjectId(string projectId)
        {
            var result = await Mediator.Send(new GetInterstitialAdModelsByProjectIdQuery
            {
                ProjectId = projectId
            });
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }


        /// <summary>
        ///     Add InterstielAdModel.
        /// </summary>
        /// <param name="createInterstitialAdModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateInterstitialAdModelCommand createInterstitialAdModel)
        {
            var result = await Mediator.Send(createInterstitialAdModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     Update InterstielAdModel.
        /// </summary>
        /// <param name="updateInterstitialAdModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateInterstitialAdModelCommand updateInterstitialAdModel)
        {
            var result = await Mediator.Send(updateInterstitialAdModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     Delete InterstielAdModel.
        /// </summary>
        /// <param name="deleteInterstitialAdModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteInterstitialAdModelCommand deleteInterstitialAdModel)
        {
            var result = await Mediator.Send(deleteInterstitialAdModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}