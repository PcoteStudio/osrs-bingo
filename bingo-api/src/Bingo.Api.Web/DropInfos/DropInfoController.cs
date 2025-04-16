using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.DropInfos;
using Bingo.Api.Core.Features.DropInfos.Arguments;
using Bingo.Api.Core.Features.DropInfos.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.DropInfos;

[Route("/api/drops")]
[ApiController]
public class DropInfoController(
    IPermissionServiceHelper permissionServiceHelper,
    IDropInfoService dropService,
    IDropInfoServiceHelper dropServiceHelper,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{dropId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DropInfoResponse>> GetDropInfoAsync([FromRoute] int dropId)
    {
        try
        {
            var drop = await dropServiceHelper.GetRequiredCompleteByIdAsync(dropId);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<DropInfoResponse>(drop));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case DropInfoNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DropInfoResponse>>> GetDropInfosAsync()
    {
        var drop = await dropService.GetDropInfosAsync();
        return StatusCode(StatusCodes.Status200OK, mapper.Map<List<DropInfoResponse>>(drop));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<DropInfoResponse>> CreateDropInfoAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromBody] DropInfoCreateArguments args)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, ["drop.create"]);
            var drop = await dropService.CreateDropInfoAsync(args);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<DropInfoResponse>(drop));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case DropInfoAlreadyExistsException:
                    throw new HttpException(StatusCodes.Status400BadRequest, ex);
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
    public async Task<ActionResult<DropInfoResponse>> UpdateDropInfoAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int dropId,
        [FromBody] DropInfoUpdateArguments args)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, ["drop.update"]);
            var drop = await dropService.UpdateDropInfoAsync(dropId, args);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<DropInfoResponse>(drop));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case DropInfoNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpDelete("{dropId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DropInfoResponse>> RemoveDropInfoAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int dropId)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, ["drop.delete"]);
            var drop = await dropService.RemoveDropInfoAsync(dropId);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<DropInfoResponse>(drop));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case DropInfoNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }
}