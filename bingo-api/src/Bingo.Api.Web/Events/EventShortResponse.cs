using JetBrains.Annotations;

namespace Bingo.Api.Web.Events;

[PublicAPI]
public class EventShortResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
}