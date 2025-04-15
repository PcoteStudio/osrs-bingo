using Bingo.Api.Web.Teams;
using Bingo.Api.Web.Users;

namespace Bingo.Api.Web.Events;

[Serializable]
public class EventResponse : EventShortResponse
{
    public List<TeamResponse> Teams { get; set; } = [];
    public List<UserPublicResponse> Administrators { get; set; } = [];
}