using Bingo.Api.Web.Events;
using Bingo.Api.Web.Players;
using JetBrains.Annotations;

namespace Bingo.Api.Web.Teams;

[PublicAPI]
public class TeamResponse : TeamShortResponse
{
    public EventShortResponse Event { get; set; }
    public List<PlayerResponse> Players { get; set; } = [];
}