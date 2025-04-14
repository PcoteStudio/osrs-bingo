namespace Bingo.Api.Core.Features.Players.Exceptions;

public class PlayerNotFoundException(int playerId) : Exception($"Player {playerId} not found.")
{
    public int PlayerId { get; } = playerId;
}