using Bingo.Api.Web.Players;

namespace Bingo.Api.Web.Teams;

[Serializable]
public class TeamResponse
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PlayerResponse> Players { get; set; } = [];
}