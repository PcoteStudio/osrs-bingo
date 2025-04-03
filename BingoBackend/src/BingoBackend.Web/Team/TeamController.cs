using AutoMapper;
using BingoBackend.Core.Features.Teams;
using Microsoft.AspNetCore.Mvc;

namespace BingoBackend.Web.Team;

public class TeamController(ITeamService teamService, IMapper mapper) : ControllerBase
{
    [HttpGet("/api/teams")]
    public async Task<ActionResult<TeamResponse[]>> ListTeams()
    {
        var teams = await teamService.ListTeamsAsync();
        return StatusCode(StatusCodes.Status200OK, teams.Select(mapper.Map<TeamResponse>).ToArray());
    }

    [HttpGet("/api/teams/{teamId:min(0)}")]
    public async Task<ActionResult<TeamResponse>> GetTeam([FromRoute] int teamId)
    {
        var team = await teamService.GetTeamAsync(teamId);
        if (team is null) return NotFound();
        return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
    }

    [HttpPost("/api/teams")]
    public ActionResult<TeamResponse> CreateTeam([FromBody] TeamCreateArguments arguments)
    {
        // EnsureIsAdmin();
        var team = teamService.CreateTeam(arguments);
        return StatusCode(StatusCodes.Status201Created, mapper.Map<TeamResponse>(team));
    }

    [HttpPut("/api/teams/{teamId:min(0)}")]
    public ActionResult<TeamResponse> UpdateTeam(
        [FromRoute] int teamId, [FromBody] TeamUpdateArguments arguments)
    {
        // EnsureIsAdmin();
        throw new NotImplementedException();
    }

    [HttpPut("/api/teams/{teamId:min(0)}/players")]
    public ActionResult<TeamResponse> UpdateTeamPlayers(
        [FromRoute] int teamId,
        [FromBody] TeamCreateArguments arguments)
    {
        // EnsureIsAdmin();
        throw new NotImplementedException();
    }

    [HttpPost("/api/teams/{teamId:min(0)}/players")]
    public ActionResult<TeamResponse> AddTeamPlayers(
        [FromRoute] int teamId,
        [FromBody] TeamCreateArguments arguments)
    {
        // EnsureIsAdmin();
        throw new NotImplementedException();
    }
}