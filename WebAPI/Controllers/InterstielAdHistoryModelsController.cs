
using Business.Handlers.InterstielAdHistoryModels.Commands;
using Business.Handlers.InterstielAdHistoryModels.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Entities.Concrete;
using System.Collections.Generic;
using Core.Utilities.Results;
using MongoDB.Bson;
namespace WebAPI.Controllers
{
    /// <summary>
    /// InterstielAdHistoryModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InterstielAdHistoryModelsController : BaseApiController
    {
        ///<summary>
        ///List InterstielAdHistoryModels
        ///</summary>
        ///<remarks>InterstielAdHistoryModels</remarks>
        ///<return>List InterstielAdHistoryModels</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InterstielAdHistoryModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetInterstielAdHistoryModelsQuery());
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        } 
        
        ///<summary>
        ///List InterstielAdHistoryModels
        ///</summary>
        ///<remarks>InterstielAdHistoryModels</remarks>
        ///<return>List InterstielAdHistoryModels</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InterstielAdHistoryModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getListByProjectId")]
        public async Task<IActionResult> GetListByProjectId(string projectId)
        {
            var result = await Mediator.Send(new GetInterstielAdHistoryModelByProjectIdQuery
            {
                ProjectId = projectId
            });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>InterstielAdHistoryModels</remarks>
        ///<return>InterstielAdHistoryModels List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InterstielAdHistoryModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(string objectId)
        {
            var result = await Mediator.Send(new GetInterstielAdHistoryModelQuery { ObjectId = objectId });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add InterstielAdHistoryModel.
        /// </summary>
        /// <param name="createInterstielAdHistoryModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateInterstielAdHistoryModelCommand createInterstielAdHistoryModel)
        {
            var result = await Mediator.Send(createInterstielAdHistoryModel);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update InterstielAdHistoryModel.
        /// </summary>
        /// <param name="updateInterstielAdHistoryModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateInterstielAdHistoryModelCommand updateInterstielAdHistoryModel)
        {
            var result = await Mediator.Send(updateInterstielAdHistoryModel);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete InterstielAdHistoryModel.
        /// </summary>
        /// <param name="deleteInterstielAdHistoryModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteInterstielAdHistoryModelCommand deleteInterstielAdHistoryModel)
        {
            var result = await Mediator.Send(deleteInterstielAdHistoryModel);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
