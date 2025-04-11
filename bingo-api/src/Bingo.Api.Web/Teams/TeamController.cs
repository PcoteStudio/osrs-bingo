using AutoMapper;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Teams;

[Route("/api/teams")]
[ApiController]
public class TeamController(ITeamService teamService, IMapper mapper, ILogger<TeamController> logger) : ControllerBase
{
    [HttpGet("{teamId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> GetTeamAsync([FromRoute] int teamId)
    {
        try
        {
            var team = await teamService.GetRequiredTeamAsync(teamId);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (TeamNotFoundException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return NotFound();
        }
    }

    [HttpPut("{teamId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> UpdateTeamAsync(
        [FromRoute] int eventId, [FromRoute] int teamId, [FromBody] TeamUpdateArguments args)
    {
        try
        {
            // EnsureIsAdmin();
            var team = await teamService.UpdateTeamAsync(teamId, args);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (TeamNotFoundException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return NotFound();
        }
    }

    [HttpPut("{teamId:min(0)}/players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<TeamResponse>> UpdateTeamPlayersAsync(
        [FromRoute] int eventId, [FromRoute] int teamId,
        [FromBody] TeamPlayersArguments args)
    {
        // EnsureIsAdmin();
        throw new NotImplementedException();
    }

    [HttpPost("{teamId:min(0)}/players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> AddTeamPlayersAsync(
        [FromRoute] int eventId, [FromRoute] int teamId,
        [FromBody] TeamPlayersArguments args)
    {
        try
        {
            // EnsureIsAdmin();
            var team = await teamService.AddTeamPlayersAsync(teamId, args);

            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (TeamNotFoundException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return NotFound();
        }
    }

    [HttpDelete("{teamId:min(0)}/players/{playerName:minlength(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> RemoveTeamPlayerAsync(
        [FromRoute] int eventId, [FromRoute] int teamId, [FromRoute] string playerName)
    {
        try
        {
            // EnsureIsAdmin();
            var team = await teamService.RemoveTeamPlayerAsync(teamId, playerName);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (TeamNotFoundException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return NotFound();
        }
    }
}