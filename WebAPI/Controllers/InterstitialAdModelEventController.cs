using Business.Services.NotificationEvents;
using Microsoft.AspNetCore.Mvc;
using IResult = Core.Utilities.Results.IResult;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InterstitialAdModelEventController : BaseApiController
{
    /// <summary>
    ///     List InterstitialAdModel
    /// </summary>
    /// <remarks>InterstitialAdModels</remarks>
    /// <return>List InterstitialAdModels</return>
    /// <response code="200"></response>
    [DisableRequestSizeLimit]
    [Produces("application/json", "text/plain")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
    [HttpGet]
    public async Task<IActionResult> Send(long projectId)
    {
        var result = await Mediator.Send(new SendInterstitialNotificationEvent
        {
            ProjectId = projectId,
        });
        if (result.Success) return Ok(result);
        return BadRequest(result);
    }


}