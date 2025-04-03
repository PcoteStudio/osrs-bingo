using BingoBackend.Web.Players;

namespace BingoBackend.Web.Teams;

public class TeamResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PlayerResponse> Players { get; set; } = [];
}