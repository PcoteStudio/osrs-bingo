using Bingo.Api.Core.Features.Events.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Events;

public interface IEventFactory
{
    EventEntity Create(EventCreateArguments args);
}

public class EventFactory : IEventFactory
{
    public EventEntity Create(EventCreateArguments args)
    {
        return new EventEntity
        {
            Name = args.Name,
            StartTime = args.StartTime,
            EndTime = args.EndTime
        };
    }
}