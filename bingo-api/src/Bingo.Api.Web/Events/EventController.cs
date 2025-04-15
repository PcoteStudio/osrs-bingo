using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Events;
using Bingo.Api.Core.Features.Events.Arguments;
using Bingo.Api.Core.Features.Events.Exceptions;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Web.Generic.Exceptions;
using Bingo.Api.Web.Teams;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Events;

[Route("/api/events")]
[ApiController]
public class EventController(
    IEventService eventService,
    IEventServiceHelper eventServiceHelper,
    IPermissionServiceHelper permissionServiceHelper,
    ITeamService teamService,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EventResponse>>> GetEventsAsync()
    {
        var events = await eventService.GetEventsAsync();
        return StatusCode(StatusCodes.Status200OK, mapper.Map<List<EventResponse>>(events));
    }

    [HttpGet("{eventId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventResponse>> GetEventAsync([FromRoute] int eventId)
    {
        try
        {
            var events = await eventServiceHelper.GetRequiredByIdAsync(eventId);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<List<EventResponse>>(events));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case EventNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TeamResponse>> CreateEventAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromBody] EventCreateArguments args)
    {
        permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, []);
        var user = (identityContainer.Identity as UserIdentity)!.User;
        var eventEntity = await eventService.CreateEventAsync(user, args);
        return StatusCode(StatusCodes.Status201Created, mapper.Map<EventResponse>(eventEntity));
    }

    [HttpPost("{eventId:min(0)}/teams")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> CreateTeamAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int eventId,
        [FromBody] TeamCreateArguments args)
    {
        try
        {
            await eventServiceHelper.EnsureIsEventAdminAsync(identityContainer.Identity, eventId);
            var team = await teamService.CreateTeamAsync(eventId, args);
            return StatusCode(StatusCodes.Status201Created, mapper.Map<TeamResponse>(team));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserIsNotAnEventAdminException:
                    throw new HttpException(StatusCodes.Status403Forbidden, ex);
                case EventNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpPut("{eventId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamResponse>> UpdateEventAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int eventId,
        [FromBody] EventUpdateArguments args)
    {
        try
        {
            await eventServiceHelper.EnsureIsEventAdminAsync(identityContainer.Identity, eventId);
            var team = await eventService.UpdateEventAsync(eventId, args);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<EventResponse>(team));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserIsNotAnEventAdminException:
                    throw new HttpException(StatusCodes.Status403Forbidden, ex);
                case EventNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }
}