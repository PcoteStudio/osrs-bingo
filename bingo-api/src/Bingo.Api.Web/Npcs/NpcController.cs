using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Npcs;
using Bingo.Api.Core.Features.Npcs.Arguments;
using Bingo.Api.Core.Features.Npcs.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Npcs;

[Route("/api/npcs")]
[ApiController]
public class NpcController(
    IPermissionServiceHelper permissionServiceHelper,
    INpcService npcService,
    INpcServiceHelper npcServiceHelper,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{npcId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NpcResponse>> GetNpcAsync([FromRoute] int npcId)
    {
        try
        {
            var npc = await npcServiceHelper.GetRequiredCompleteByIdAsync(npcId);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<NpcResponse>(npc));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NpcNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<NpcResponse>>> GetNpcsAsync()
    {
        var npc = await npcService.GetNpcsAsync();
        return StatusCode(StatusCodes.Status200OK, mapper.Map<List<NpcResponse>>(npc));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<NpcResponse>> CreateNpcAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromBody] NpcCreateArguments args)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "npc.create");
            var npc = await npcService.CreateNpcAsync(args);
            return StatusCode(StatusCodes.Status201Created, mapper.Map<NpcResponse>(npc));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NpcAlreadyExistsException:
                    throw new HttpException(StatusCodes.Status400BadRequest, ex);
                default:
                    throw;
            }
        }
    }

    [HttpPut("{npcId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NpcResponse>> UpdateNpcAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int npcId,
        [FromBody] NpcUpdateArguments args)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "npc.update");
            var npc = await npcService.UpdateNpcAsync(npcId, args);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<NpcResponse>(npc));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NpcNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpDelete("{npcId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NpcResponse>> RemoveNpcAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int npcId)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "npc.delete");
            var npc = await npcService.RemoveNpcAsync(npcId);
            return StatusCode(StatusCodes.Status204NoContent, mapper.Map<NpcResponse>(npc));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NpcNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }
}