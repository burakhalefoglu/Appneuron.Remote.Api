﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Handlers.InterstielAdHistoryModels.Commands;
using Business.Handlers.InterstielAdHistoryModels.Queries;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    ///     InterstielAdHistoryModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InterstielAdHistoryModelsController : BaseApiController
    {
        /// <summary>
        ///     List InterstielAdHistoryModels
        /// </summary>
        /// <remarks>InterstielAdHistoryModels</remarks>
        /// <return>List InterstielAdHistoryModels</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IDataResult<IEnumerable<InterstielAdHistoryModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getListByProjectId")]
        public async Task<IActionResult> GetListByProjectId(string projectId)
        {
            var result = await Mediator.Send(new GetInterstielAdHistoryModelByProjectIdQuery
            {
                ProjectId = projectId
            });
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     Add InterstielAdHistoryModel.
        /// </summary>
        /// <param name="createInterstielAdHistoryModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost]
        public async Task<IActionResult> Add(
            [FromBody] CreateInterstielAdHistoryModelCommand createInterstielAdHistoryModel)
        {
            var result = await Mediator.Send(createInterstielAdHistoryModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}