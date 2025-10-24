using Bingo.Api.Web.Teams;
using Bingo.Api.Web.Users;
using JetBrains.Annotations;

namespace Bingo.Api.Web.Events;

[PublicAPI]
public class EventResponse : EventShortResponse
{
    public List<TeamResponse> Teams { get; set; } = [];
    public List<UserPublicResponse> Administrators { get; set; } = [];
}