using BingoBackend.Core.Features.Team;
using Microsoft.AspNetCore.Mvc;

namespace BingoBackend.Web.Team;

public class TeamController(TeamService teamService) : ControllerBase
{
    [HttpGet("/api/teams")]
    public ActionResult<TeamResponse[]> ListTeams()
    {
        var teams = teamService.ListTeams();
        // TODO Use Automapper
        return StatusCode(StatusCodes.Status200OK,
            teams.Select(t => new TeamResponse { Id = t.Id, Name = t.Name }).ToArray());
    }

    [HttpPost("/api/teams")]
    public ActionResult<TeamResponse> CreateTeam([FromBody] CreateTeamRequest request)
    {
        // EnsureIsAdmin();
        var team = teamService.CreateTeam(request.Name);
        // TODO Use Automapper
        return StatusCode(StatusCodes.Status201Created, new TeamResponse { Id = team.Id, Name = team.Name });
    }
}