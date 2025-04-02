using AutoMapper;
using BingoBackend.Core.Features.Team;
using Microsoft.AspNetCore.Mvc;

namespace BingoBackend.Web.Team;

public class TeamController(ITeamService teamService, IMapper mapper) : ControllerBase
{
    [HttpGet("/api/teams")]
    public async Task<ActionResult<TeamResponse[]>> ListTeams()
    {
        var teams = await teamService.ListTeams();
        return StatusCode(StatusCodes.Status200OK, teams.Select(mapper.Map<TeamResponse>).ToArray());
    }

    [HttpPost("/api/teams")]
    public ActionResult<TeamResponse> CreateTeam([FromBody] TeamCreateArguments arguments)
    {
        // EnsureIsAdmin();
        var team = teamService.CreateTeam(arguments);
        return StatusCode(StatusCodes.Status201Created, mapper.Map<TeamResponse>(team));
    }
}