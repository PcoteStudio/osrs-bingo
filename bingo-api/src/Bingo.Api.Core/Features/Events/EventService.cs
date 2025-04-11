using System.Security.Claims;
using Bingo.Api.Core.Features.Events.Arguments;
using Bingo.Api.Core.Features.Events.Exceptions;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Events;

public interface IEventService
{
    Task<UserEntity> EnsureIsEventAdminAsync(ClaimsPrincipal principal, int eventId);
    Task<EventEntity> CreateEventAsync(UserEntity user, EventCreateArguments args);
    Task<EventEntity> UpdateEventAsync(int eventId, EventUpdateArguments args);
    Task<EventEntity> GetRequiredEventAsync(int eventId);
    Task<List<EventEntity>> GetEventsAsync();
}

public class EventService(
    IEventFactory eventFactory,
    IEventRepository eventRepository,
    IEventUtil eventUtil,
    IUserService userService,
    ApplicationDbContext dbContext
) : IEventService
{
    public async Task<UserEntity> EnsureIsEventAdminAsync(ClaimsPrincipal principal, int eventId)
    {
        var user = await userService.GetRequiredMeAsync(principal);
        var eventEntity = await GetRequiredEventAsync(eventId);
        if (!eventEntity.Administrators.Contains(user))
            throw new UserIsNotAnEventAdminException(eventEntity.Id, principal.Identity!.Name!);
        return user;
    }

    public async Task<EventEntity> CreateEventAsync(UserEntity user, EventCreateArguments args)
    {
        var eventEntity = eventFactory.Create(args);
        eventEntity.Administrators.Add(user);
        eventRepository.Add(eventEntity);
        await dbContext.SaveChangesAsync();
        return eventEntity;
    }

    public async Task<EventEntity> UpdateEventAsync(int eventId, EventUpdateArguments args)
    {
        var eventEntity = await GetRequiredEventAsync(eventId);
        eventUtil.UpdateEvent(eventEntity, args);
        dbContext.Update(eventEntity);
        await dbContext.SaveChangesAsync();
        return eventEntity;
    }

    public async Task<EventEntity> GetRequiredEventAsync(int eventId)
    {
        var eventEntity = await eventRepository.GetCompleteByIdAsync(eventId);
        if (eventEntity == null) throw new EventNotFoundException(eventId);
        return eventEntity;
    }

    public Task<List<EventEntity>> GetEventsAsync()
    {
        return eventRepository.GetAllAsync();
    }
}