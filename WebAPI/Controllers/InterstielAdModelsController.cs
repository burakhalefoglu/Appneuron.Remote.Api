
using Business.Handlers.InterstielAdModels.Commands;
using Business.Handlers.InterstielAdModels.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Entities.Concrete;
using System.Collections.Generic;
using MongoDB.Bson;
using Core.Utilities.Results;

namespace WebAPI.Controllers
{
    /// <summary>
    /// InterstielAdModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InterstielAdModelsController : BaseApiController
    {
        ///<summary>
        ///List InterstielAdModels
        ///</summary>
        ///<remarks>InterstielAdModels</remarks>
        ///<return>List InterstielAdModels</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InterstielAdModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IDataResult<InterstielAdModel>))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetInterstielAdModelsQuery());
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        ///<summary>
        ///List InterstielAdModels
        ///</summary>
        ///<remarks>InterstielAdModels</remarks>
        ///<return>List InterstielAdModels</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InterstielAdModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IDataResult<InterstielAdModel>))]
        [HttpGet("getByProjectId")]
        public async Task<IActionResult> GetByProjectId(string projectId)
        {
            var result = await Mediator.Send(new GetInterstielAdModelsByProjectIdQuery
            {
                ProjectId = projectId
            });
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>InterstielAdModels</remarks>
        ///<return>InterstielAdModels List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InterstielAdModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(string objectId)
        {
            var result = await Mediator.Send(new GetInterstielAdModelQuery { ObjectId = objectId });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add InterstielAdModel.
        /// </summary>
        /// <param name="createInterstielAdModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateInterstielAdModelCommand createInterstielAdModel)
        {
            var result = await Mediator.Send(createInterstielAdModel);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Update InterstielAdModel.
        /// </summary>
        /// <param name="updateInterstielAdModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateInterstielAdModelCommand updateInterstielAdModel)
        {
            var result = await Mediator.Send(updateInterstielAdModel);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Delete InterstielAdModel.
        /// </summary>
        /// <param name="deleteInterstielAdModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteInterstielAdModelCommand deleteInterstielAdModel)
        {
            var result = await Mediator.Send(deleteInterstielAdModel);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
