using Bingo.Api.Core.Features.Players.Exceptions;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerServiceHelper
{
    Task EnsurePlayerDoesNotExistByNameAsync(string name);
    Task<PlayerEntity> GetRequiredCompletePlayerAsync(int playerId);
    Task<PlayerEntity> GetRequiredCompletePlayerByNameAsync(string name);
}

public class PlayerServiceHelper(
    IPlayerRepository playerRepository
) : IPlayerServiceHelper
{
    public async Task EnsurePlayerDoesNotExistByNameAsync(string name)
    {
        var player = await playerRepository.GetByNameAsync(name);
        if (player is not null) throw new PlayerAlreadyExistsException(player.Name);
    }

    public virtual async Task<PlayerEntity> GetRequiredCompletePlayerAsync(int playerId)
    {
        var player = await playerRepository.GetCompleteByIdAsync(playerId);
        if (player is null) throw new PlayerNotFoundException(playerId);
        return player;
    }

    public virtual async Task<PlayerEntity> GetRequiredCompletePlayerByNameAsync(string name)
    {
        var player = await playerRepository.GetCompleteByNameAsync(name);
        if (player is null) throw new PlayerNotFoundException(name);
        return player;
    }
}