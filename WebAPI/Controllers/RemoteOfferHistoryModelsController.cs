
using Business.Handlers.RemoteOfferHistoryModels.Commands;
using Business.Handlers.RemoteOfferHistoryModels.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Entities.Concrete;
using System.Collections.Generic;
using MongoDB.Bson;
namespace WebAPI.Controllers
{
    /// <summary>
    /// RemoteOfferHistoryModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteOfferHistoryModelsController : BaseApiController
    {
        ///<summary>
        ///List RemoteOfferHistoryModels
        ///</summary>
        ///<remarks>RemoteOfferHistoryModels</remarks>
        ///<return>List RemoteOfferHistoryModels</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RemoteOfferHistoryModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetRemoteOfferHistoryModelsQuery());
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        ///<summary>
        ///List RemoteOfferModels
        ///</summary>
        ///<remarks>RemoteOfferModels</remarks>
        ///<return>List RemoteOfferModels</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RemoteOfferHistoryModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<RemoteOfferHistoryModel>))]
        [HttpGet("getByProjectId")]
        public async Task<IActionResult> GetByProjectId(string projectId)
        {
            var result = await Mediator.Send(new GetOfferHistoryModelsByProjectIdQuery
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
        ///<remarks>RemoteOfferHistoryModels</remarks>
        ///<return>RemoteOfferHistoryModels List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RemoteOfferHistoryModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(string objectId)
        {
            var result = await Mediator.Send(new GetRemoteOfferHistoryModelQuery { ObjectId = objectId });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add RemoteOfferHistoryModel.
        /// </summary>
        /// <param name="createRemoteOfferHistoryModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateRemoteOfferHistoryModelCommand createRemoteOfferHistoryModel)
        {
            var result = await Mediator.Send(createRemoteOfferHistoryModel);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update RemoteOfferHistoryModel.
        /// </summary>
        /// <param name="updateRemoteOfferHistoryModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRemoteOfferHistoryModelCommand updateRemoteOfferHistoryModel)
        {
            var result = await Mediator.Send(updateRemoteOfferHistoryModel);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete RemoteOfferHistoryModel.
        /// </summary>
        /// <param name="deleteRemoteOfferHistoryModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteRemoteOfferHistoryModelCommand deleteRemoteOfferHistoryModel)
        {
            var result = await Mediator.Send(deleteRemoteOfferHistoryModel);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
