using Bingo.Api.Core.Features.Players.Exceptions;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerServiceHelper
{
    Task EnsurePlayerDoesNotExistByNameAsync(string name);
    Task<PlayerEntity> GetRequiredCompletePlayerAsync(int teamId);
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

    public virtual async Task<PlayerEntity> GetRequiredCompletePlayerAsync(int teamId)
    {
        var player = await playerRepository.GetCompleteByIdAsync(teamId);
        if (player == null) throw new PlayerNotFoundException(teamId);
        return player;
    }
}