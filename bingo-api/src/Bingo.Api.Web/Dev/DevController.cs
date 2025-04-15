using AutoMapper;
using Bingo.Api.Core.Features.Events;
using Bingo.Api.Core.Features.Events.Arguments;
using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Players.Arguments;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Shared;
using Bingo.Api.Web.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Dev;

[Route("/api/dev")]
[ApiController]
public class DevController(
    IUserService userService,
    IEventService eventService,
    ITeamService teamService,
    IPlayerService playerService,
    IMapper mapper,
    ILogger<DevController> logger,
    IHostEnvironment environment)
    : ControllerBase
{
    [Authorize]
    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> Ping()
    {
        if (!environment.IsDevelopment()) return StatusCode(StatusCodes.Status403Forbidden);
        return StatusCode(StatusCodes.Status200OK, "pong");
    }

    [Authorize]
    [HttpPost("seed")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<EventResponse>> SeedAsync()
    {
        if (!environment.IsDevelopment()) return StatusCode(StatusCodes.Status403Forbidden);
        logger.LogInformation("Seeding an event");

        var user = await userService.GetRequiredMeAsync(User);
        var newEvent = await eventService.CreateEventAsync(user, new EventCreateArguments
        {
            Name = RandomUtil.GetPrefixedRandomHexString("EventName_", Random.Shared.Next(5, 20))
        });
        for (var i = 0; i < Random.Shared.Next(1, 10); i++)
        {
            var newTeam = await teamService.CreateTeamAsync(newEvent.Id, new TeamCreateArguments
            {
                Name = RandomUtil.GetPrefixedRandomHexString("TeamName_", Random.Shared.Next(5, 20))
            });
            for (var j = 0; j < Random.Shared.Next(1, 20); j++)
            {
                var newPlayer = await playerService.CreatePlayerAsync(new PlayerCreateArguments
                {
                    Name = RandomUtil.GetPrefixedRandomHexString("PlayerName_", Random.Shared.Next(5, 20)),
                    TeamIds = [newTeam.Id]
                });
            }
        }

        return StatusCode(StatusCodes.Status201Created, mapper.Map<EventResponse>(newEvent));
    }
}