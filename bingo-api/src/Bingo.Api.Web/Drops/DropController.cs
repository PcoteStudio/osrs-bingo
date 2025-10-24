using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Drops;
using Bingo.Api.Core.Features.Drops.Arguments;
using Bingo.Api.Core.Features.Drops.Exceptions;
using Bingo.Api.Core.Features.Items.Exceptions;
using Bingo.Api.Core.Features.Npcs.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Drops;

[Route("/api/drops")]
[ApiController]
public class DropController(
    IPermissionServiceHelper permissionServiceHelper,
    IDropService dropService,
    IDropServiceHelper dropServiceHelper) : ControllerBase
{
    [HttpGet("{dropId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DropResponse>> GetDropAsync([FromRoute] int dropId)
    {
        try
        {
            var drop = await dropServiceHelper.GetRequiredCompleteByIdAsync(dropId);
            return StatusCode(StatusCodes.Status200OK, drop.ToResponse());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case DropNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DropResponse>>> GetDropsAsync()
    {
        var drops = await dropService.GetDropsAsync();
        return StatusCode(StatusCodes.Status200OK, drops.ToResponseList());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<DropResponse>> CreateDropAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromBody] DropCreateArguments args)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "drop.create");
            var drop = await dropService.CreateDropAsync(args);
            return StatusCode(StatusCodes.Status201Created, drop.ToResponse());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case ItemNotFoundException or NpcNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                case DropAlreadyExistsException:
                    throw new HttpException(StatusCodes.Status409Conflict, ex);
                default:
                    throw;
            }
        }
    }

    [HttpPut("{dropId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<DropResponse>> UpdateDropAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int dropId,
        [FromBody] DropUpdateArguments args)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "drop.update");
            var drop = await dropService.UpdateDropAsync(dropId, args);
            return StatusCode(StatusCodes.Status200OK, drop.ToResponse());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case DropNotFoundException or ItemNotFoundException or NpcNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                case DropAlreadyExistsException:
                    throw new HttpException(StatusCodes.Status409Conflict, ex);
                default:
                    throw;
            }
        }
    }

    [HttpDelete("{dropId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DropResponse>> RemoveDropAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int dropId)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "drop.delete");
            var drop = await dropService.RemoveDropAsync(dropId);
            return StatusCode(StatusCodes.Status204NoContent, drop.ToResponse());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case DropNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }
}