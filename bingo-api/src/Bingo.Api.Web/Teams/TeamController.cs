using AutoMapper;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Teams;

[Route("/api/teams")]
[ApiController]
public class TeamController(ITeamService teamService, IMapper mapper) : ControllerBase
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
        catch (Exception ex)
        {
            switch (ex)
            {
                case TeamNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [Authorize]
    [HttpPut("{teamId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> UpdateTeamAsync([FromRoute] int teamId,
        [FromBody] TeamUpdateArguments args)
    {
        try
        {
            await teamService.EnsureIsTeamAdminAsync(User, teamId);
            var team = await teamService.UpdateTeamAsync(teamId, args);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserIsNotATeamAdminException:
                    throw new HttpException(StatusCodes.Status403Forbidden, ex);
                case TeamNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [Authorize]
    [HttpPut("{teamId:min(0)}/players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> UpdateTeamPlayersAsync([FromRoute] int teamId,
        [FromBody] TeamPlayersArguments args)
    {
        await teamService.EnsureIsTeamAdminAsync(User, teamId);
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost("{teamId:min(0)}/players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> AddTeamPlayersAsync([FromRoute] int teamId,
        [FromBody] TeamPlayersArguments args)
    {
        try
        {
            await teamService.EnsureIsTeamAdminAsync(User, teamId);
            var team = await teamService.AddTeamPlayersAsync(teamId, args);

            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserIsNotATeamAdminException:
                    throw new HttpException(StatusCodes.Status403Forbidden, ex);
                case TeamNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [Authorize]
    [HttpDelete("{teamId:min(0)}/players/{playerName:minlength(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> RemoveTeamPlayerAsync([FromRoute] int teamId,
        [FromRoute] string playerName)
    {
        try
        {
            await teamService.EnsureIsTeamAdminAsync(User, teamId);
            var team = await teamService.RemoveTeamPlayerAsync(teamId, playerName);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<TeamResponse>(team));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserIsNotATeamAdminException:
                    throw new HttpException(StatusCodes.Status403Forbidden, ex);
                case TeamNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }
}