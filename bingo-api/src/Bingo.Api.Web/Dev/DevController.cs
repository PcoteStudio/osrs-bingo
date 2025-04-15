using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Events;
using Bingo.Api.Core.Features.Events.Arguments;
using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Players.Arguments;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Shared;
using Bingo.Api.Web.Events;
using Bingo.Api.Web.Generic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Dev;

[Route("/api/dev")]
[ApiController]
public class DevController(
    IPermissionServiceHelper permissionServiceHelper,
    IEventService eventService,
    ITeamService teamService,
    IPlayerService playerService,
    IMapper mapper,
    ILogger<DevController> logger,
    IHostEnvironment environment)
    : ControllerBase
{
    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> Ping([FromServices] IdentityContainer identityContainer)
    {
        EnsureHasAccess(identityContainer.Identity, []);
        return StatusCode(StatusCodes.Status200OK, "pong");
    }

    [HttpPost("seed")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<EventResponse>> SeedAsync([FromServices] IdentityContainer identityContainer)
    {
        EnsureHasAccess(identityContainer.Identity, []);
        var user = (identityContainer.Identity as UserIdentity)!.User;

        logger.LogInformation("Seeding an event");

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

    private void EnsureHasAccess(IIdentity? identity, List<string> permissions)
    {
        if (!environment.IsDevelopment())
            throw new HttpException(StatusCodes.Status404NotFound);

        permissionServiceHelper.EnsureHasPermissions(identity, permissions);
    }
}