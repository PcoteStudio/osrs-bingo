using Bingo.Api.Web.Teams;
using JetBrains.Annotations;

namespace Bingo.Api.Web.Players;

[PublicAPI]
public class PlayerResponse : PlayerShortResponse
{
    public List<TeamShortResponse> Teams { get; set; } = [];

    public IEnumerable<int> TeamIds => Teams.Select(t => t.Id);
}