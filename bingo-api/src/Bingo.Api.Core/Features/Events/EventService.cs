using Bingo.Api.Core.Features.Events.Arguments;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Events;

public interface IEventService
{
    Task<EventEntity> CreateEventAsync(UserEntity user, EventCreateArguments args);
    Task<EventEntity> UpdateEventAsync(int eventId, EventUpdateArguments args);
    Task<List<EventEntity>> GetEventsAsync();
}

public class EventService(
    IEventFactory eventFactory,
    IEventRepository eventRepository,
    IEventUtil eventUtil,
    IEventServiceHelper eventServiceHelper,
    ApplicationDbContext dbContext
) : IEventService
{
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
        var eventEntity = await eventServiceHelper.GetRequiredByIdAsync(eventId);
        eventUtil.UpdateEvent(eventEntity, args);
        dbContext.Update(eventEntity);
        await dbContext.SaveChangesAsync();
        return eventEntity;
    }

    public Task<List<EventEntity>> GetEventsAsync()
    {
        return eventRepository.GetAllAsync();
    }
}