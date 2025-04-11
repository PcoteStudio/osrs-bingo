namespace Bingo.Api.Core.Features.Events.Exceptions;

public class EventNotFoundException(int eventId) : Exception($"Event {eventId} not found.")
{
    public int EventId { get; } = eventId;
}