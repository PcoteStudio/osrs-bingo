using Bingo.Api.Web.Teams;

namespace Bingo.Api.Web.Events;

public class EventResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public List<TeamResponse> Teams { get; set; } = [];
}