
using Business.Handlers.RemoteOfferModels.Commands;
using Business.Handlers.RemoteOfferModels.Queries;
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
    /// RemoteOfferModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteOfferModelsController : BaseApiController
    {
        ///<summary>
        ///List RemoteOfferModels
        ///</summary>
        ///<remarks>RemoteOfferModels</remarks>
        ///<return>List RemoteOfferModels</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RemoteOfferModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetRemoteOfferModelsQuery());
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RemoteOfferModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<RemoteOfferModel>))]
        [HttpGet("getByProjectId")]
        public async Task<IActionResult> GetByProjectId(string projectId)
        {
            var result = await Mediator.Send(new GetRemoteOfferModelsByProjectIdQuery
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
        ///<remarks>RemoteOfferModels</remarks>
        ///<return>RemoteOfferModels List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RemoteOfferModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(string objectId)
        {
            var result = await Mediator.Send(new GetRemoteOfferModelQuery { ObjectId = objectId });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add RemoteOfferModel.
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
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Update RemoteOfferModel.
        /// </summary>
        /// <param name="updateRemoteOfferModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRemoteOfferModelCommand updateRemoteOfferModel)
        {
            var result = await Mediator.Send(updateRemoteOfferModel);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete RemoteOfferModel.
        /// </summary>
        /// <param name="deleteRemoteOfferModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteRemoteOfferModelCommand deleteRemoteOfferModel)
        {
            var result = await Mediator.Send(deleteRemoteOfferModel);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
