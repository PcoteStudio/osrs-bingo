using AutoMapper;
using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Players.Arguments;
using Bingo.Api.Core.Features.Players.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Players;

[Route("/api/players")]
[ApiController]
public class PlayerController(IPlayerService playerService, IMapper mapper) : ControllerBase
{
    [HttpGet("{playerId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlayerResponse>> GetPlayerAsync([FromRoute] int playerId)
    {
        try
        {
            var player = await playerService.GetRequiredCompletePlayerAsync(playerId);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<PlayerResponse>(player));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case PlayerNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpPost("{playerName:minlength(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PlayerResponse>> GetOrCreatePlayerAsync([FromRoute] string playerName)
    {
        var player = await playerService.GetOrCreatePlayerByNameAsync(playerName);
        return StatusCode(StatusCodes.Status200OK, mapper.Map<PlayerResponse>(player));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{playerId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlayerResponse>> UpdatePlayerAsync([FromRoute] int playerId,
        [FromBody] PlayerUpdateArguments args)
    {
        try
        {
            var player = await playerService.UpdatePlayerAsync(playerId, args);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<PlayerResponse>(player));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case PlayerNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{playerId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlayerResponse>> RemovePlayerAsync([FromRoute] int playerId)
    {
        try
        {
            var player = await playerService.RemovePlayerAsync(playerId);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<PlayerResponse>(player));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case PlayerNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }
}