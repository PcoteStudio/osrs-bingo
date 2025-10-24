using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Boards.MultiLayer;
using Bingo.Api.Core.Features.Boards.MultiLayer.Arguments;
using Bingo.Api.Core.Features.Events;
using Bingo.Api.Core.Features.Events.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Boards.MultiLayer;

[Route("/api/events/{eventId:min(0)}/mlboard")]
[ApiController]
public class MultiLayerBoardController(
    IEventServiceHelper eventServiceHelper,
    IPermissionServiceHelper permissionServiceHelper,
    IMultiLayerBoardService mlBoardService)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MultiLayerBoardResponse>> CreateMultiLayerBoardAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int eventId,
        [FromBody] MultiLayerBoardCreateArguments args)
    {
        try
        {
            await eventServiceHelper.EnsureIsEventAdminAsync(identityContainer.Identity, eventId);
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "board.create");
            var boardEntity = await mlBoardService.CreateMultiLayerBoardAsync(eventId, args);
            return StatusCode(StatusCodes.Status201Created, boardEntity.ToResponse());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserIsNotAnEventAdminException:
                    throw new HttpException(StatusCodes.Status403Forbidden, ex);
                case EventNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }
}