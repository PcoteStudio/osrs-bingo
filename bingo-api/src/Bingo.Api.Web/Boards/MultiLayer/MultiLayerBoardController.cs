using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Boards.MultiLayer;
using Bingo.Api.Core.Features.Boards.MultiLayer.Arguments;
using Bingo.Api.Core.Features.Events;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Boards.MultiLayer;

[Route("/api/events/{eventId:min(0)}/mlboard")]
[ApiController]
public class MultiLayerBoardController(
    IEventServiceHelper eventServiceHelper,
    IPermissionServiceHelper permissionServiceHelper,
    IMultiLayerBoardService mlBoardService,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<MultiLayerBoardResponse>> CreateMultiLayerBoardAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int eventId,
        [FromBody] MultiLayerBoardCreateArguments args)
    {
        await eventServiceHelper.EnsureIsEventAdminAsync(identityContainer.Identity, eventId);
        var eventEntity = await mlBoardService.CreateMultiLayerBoardAsync(eventId, args);
        return StatusCode(StatusCodes.Status201Created, mapper.Map<MultiLayerBoardResponse>(eventEntity));
    }
}