namespace Bingo.Api.Core.Features.Events.Arguments;

public class EventCreateArguments
{
    public string Name { get; set; } = string.Empty;

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset EndTime { get; set; }
}