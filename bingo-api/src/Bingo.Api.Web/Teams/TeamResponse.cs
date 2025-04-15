using Bingo.Api.Web.Events;
using Bingo.Api.Web.Players;

namespace Bingo.Api.Web.Teams;

[Serializable]
public class TeamResponse : TeamShortResponse
{
    public EventShortResponse Event { get; set; }
    public List<PlayerResponse> Players { get; set; } = [];
}