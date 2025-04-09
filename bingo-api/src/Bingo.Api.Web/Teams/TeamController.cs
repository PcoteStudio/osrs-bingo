using AutoMapper;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Teams;

[Route("/api/teams")]
[ApiController]
public class TeamController(ITeamService teamService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TeamResponse>>> GetTeamsAsync()
    {
        var teams = await teamService.GetTeamsAsync();
        return StatusCode(StatusCodes.Status200OK, mapper.Map<List<TeamResponse>>(teams));
    }

    [HttpGet("{teamId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> GetTeamAsync([FromRoute] int teamId)
    {
        try
        {
            var team = await teamService.GetTeamAsync(teamId);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (TeamNotFoundException ex)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<TeamResponse>> CreateTeamAsync([FromBody] TeamCreateArguments args)
    {
        // EnsureIsAdmin();
        var team = await teamService.CreateTeamAsync(args);
        return StatusCode(StatusCodes.Status201Created, mapper.Map<TeamResponse>(team));
    }

    [HttpPut("{teamId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> UpdateTeamAsync(
        [FromRoute] int teamId, [FromBody] TeamUpdateArguments args)
    {
        try
        {
            // EnsureIsAdmin();
            var team = await teamService.UpdateTeamAsync(teamId, args);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (TeamNotFoundException ex)
        {
            return NotFound();
        }
    }

    [HttpPut("{teamId:min(0)}/players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TeamResponse> UpdateTeamPlayersAsync(
        [FromRoute] int teamId,
        [FromBody] TeamPlayersArguments args)
    {
        // EnsureIsAdmin();
        throw new NotImplementedException();
    }

    [HttpPost("{teamId:min(0)}/players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> AddTeamPlayersAsync(
        [FromRoute] int teamId,
        [FromBody] TeamPlayersArguments args)
    {
        try
        {
            // EnsureIsAdmin();
            var team = await teamService.AddTeamPlayers(teamId, args);

            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (TeamNotFoundException ex)
        {
            return NotFound();
        }
    }

    [HttpDelete("{teamId:min(0)}/players/{playerId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TeamResponse> RemoveTeamPlayerAsync(
        [FromRoute] int teamId,
        [FromRoute] int playerId)
    {
        // EnsureIsAdmin();
        throw new NotImplementedException();
    }
}