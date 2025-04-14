namespace Bingo.Api.Core.Features.Players.Exceptions;

public class PlayerAlreadyExistsException(string playerName) : Exception($"Player {playerName} already exists.")
{
    public string PlayerName { get; } = playerName;
}