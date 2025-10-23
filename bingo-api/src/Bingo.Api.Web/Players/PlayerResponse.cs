using Bingo.Api.Web.Teams;

namespace Bingo.Api.Web.Players;

[Serializable]
public class PlayerResponse : PlayerShortResponse
{
    public List<TeamShortResponse> Teams { get; set; } = [];

    public IEnumerable<int> TeamIds => Teams.Select(t => t.Id);
}