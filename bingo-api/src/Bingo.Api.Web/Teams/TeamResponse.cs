using Bingo.Api.Web.Players;

namespace Bingo.Api.Web.Teams;

[Serializable]
public class TeamResponse : TeamShortResponse
{
    public List<PlayerResponse> Players { get; set; } = [];
}