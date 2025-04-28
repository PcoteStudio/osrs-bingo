namespace Bingo.Api.Core.Features.Players.Exceptions;

public class PlayerNotFoundException : Exception
{
    public PlayerNotFoundException(int playerId) : base($"Player {playerId} not found.")
    {
        PlayerId = playerId;
    }

    public PlayerNotFoundException(string playerName) : base($"Player {playerName} not found.")
    {
        PlayerName = playerName;
    }

    public int? PlayerId { get; }
    public string? PlayerName { get; }
}