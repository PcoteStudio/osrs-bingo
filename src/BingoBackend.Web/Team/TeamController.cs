using BingoBackend.Core;
using BingoBackend.Core.Features.Team;
using Microsoft.AspNetCore.Mvc;

namespace BingoBackend.Web.Team;

public class TeamController(TeamService teamService) : ControllerBase
{
    [HttpPost("/api/teams")]
    public ActionResult<TeamResponse> CreateTeam([FromBody] CreateTeamRequest request)
    {
        // EnsureIsAdmin();
        var team = teamService.CreateTeam(request.Name);
        // TODO Use Automapper
        return StatusCode(StatusCodes.Status201Created, new TeamResponse { Id = team.Id, Name = team.Name });
    }
}