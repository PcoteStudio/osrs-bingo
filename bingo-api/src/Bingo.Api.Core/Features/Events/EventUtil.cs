using Bingo.Api.Core.Features.Events.Arguments;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Events;

public interface IEventUtil
{
    void UpdateEvent(EventEntity eventEntity, EventUpdateArguments args);
}

public class EventUtil : IEventUtil
{
    public void UpdateEvent(EventEntity eventEntity, EventUpdateArguments args)
    {
        eventEntity.Name = args.Name;
        eventEntity.StartTime = args.StartTime;
        eventEntity.EndTime = args.EndTime;
    }
}