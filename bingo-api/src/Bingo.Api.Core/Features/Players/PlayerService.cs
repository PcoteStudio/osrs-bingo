using Bingo.Api.Data;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerService
{
    ValueTask<PlayerEntity?> GetPlayerAsync(int playerId);
    Task<PlayerEntity> GetOrCreatePlayerByNameAsync(string name);
    Task<List<PlayerEntity>> GetOrCreatePlayersByNamesAsync(ICollection<string> names);
    Task<List<PlayerEntity>> GetPlayersAsync();
}

public class PlayerService(
    IPlayerFactory playerFactory,
    IPlayerRepository playerRepository,
    ApplicationDbContext dbContext
) : IPlayerService
{
    public ValueTask<PlayerEntity?> GetPlayerAsync(int playerId)
    {
        return playerRepository.GetByIdAsync(playerId);
    }

    public async Task<List<PlayerEntity>> GetPlayersAsync()
    {
        return await playerRepository.GetAllAsync();
    }

    public async Task<PlayerEntity> GetOrCreatePlayerByNameAsync(string name)
    {
        var playerEntity = await playerRepository.GetByNameAsync(name);
        if (playerEntity is null)
        {
            playerEntity = playerFactory.Create(name);
            playerRepository.Add(playerEntity);
            await dbContext.SaveChangesAsync();
        }

        return playerEntity;
    }

    public async Task<List<PlayerEntity>> GetOrCreatePlayersByNamesAsync(ICollection<string> names)
    {
        var foundPlayers = await playerRepository.GetByNamesAsync(names);
        var namesNotFound = names.Except(foundPlayers.Select(x => x.Name));
        var newPlayers = namesNotFound.Select(playerFactory.Create).ToList();
        if (newPlayers.Count > 0)
        {
            playerRepository.Add(newPlayers);
            await dbContext.SaveChangesAsync();
        }

        return foundPlayers.Concat(newPlayers).ToList();
    }
}