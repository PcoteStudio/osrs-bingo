using BingoBackend.Core.Features.Players;

namespace BingoBackend.Core.Features.Teams;

public class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Player> Players { get; set; } = [];
}