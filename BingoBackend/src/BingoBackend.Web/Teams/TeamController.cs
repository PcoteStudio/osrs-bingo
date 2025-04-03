using AutoMapper;
using BingoBackend.Core.Features.Players;
using BingoBackend.Core.Features.Teams;
using BingoBackend.Core.Features.Teams.Arguments;
using BingoBackend.Core.Features.Teams.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BingoBackend.Web.Teams;

public class TeamController(ITeamService teamService, IPlayerService playerService, IMapper mapper) : ControllerBase
{
    [HttpGet("/api/teams")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TeamResponse[]>> ListTeams()
    {
        var teams = await teamService.ListTeamsAsync();
        return StatusCode(StatusCodes.Status200OK, teams.Select(mapper.Map<TeamResponse>).ToArray());
    }

    [HttpGet("/api/teams/{teamId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> GetTeam([FromRoute] int teamId)
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

    [HttpPost("/api/teams")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<TeamResponse> CreateTeam([FromBody] TeamCreateArguments args)
    {
        // EnsureIsAdmin();
        var team = teamService.CreateTeam(args);
        return StatusCode(StatusCodes.Status201Created, mapper.Map<TeamResponse>(team));
    }

    [HttpPut("/api/teams/{teamId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> UpdateTeam(
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

    [HttpPut("/api/teams/{teamId:min(0)}/players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TeamResponse> UpdateTeamPlayers(
        [FromRoute] int teamId,
        [FromBody] TeamPlayersArguments args)
    {
        // EnsureIsAdmin();
        throw new NotImplementedException();
    }

    [HttpPost("/api/teams/{teamId:min(0)}/players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> AddTeamPlayers(
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

    [HttpDelete("/api/teams/{teamId:min(0)}/players/{playerId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TeamResponse> RemoveTeamPlayer(
        [FromRoute] int teamId,
        [FromRoute] int playerId)
    {
        // EnsureIsAdmin();
        throw new NotImplementedException();
    }
}