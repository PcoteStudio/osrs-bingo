using Bingo.Api.Web.Teams;
using Bingo.Api.Web.Users;

namespace Bingo.Api.Web.Events;

[Serializable]
public class EventResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public List<TeamResponse> Teams { get; set; } = [];
    public List<UserPublicResponse> Administrators { get; set; } = [];
}