using BingoBackend.Data;
using BingoBackend.Data.Entities;

namespace BingoBackend.Core.Features.Players;

public interface IPlayerService
{
    ValueTask<PlayerEntity?> GetPlayer(int playerId);
    Task<PlayerEntity> GetOrCreatePlayerByName(string name);
    Task<List<PlayerEntity>> GetOrCreatePlayersByNames(ICollection<string> names);
    Task<List<PlayerEntity>> GetPlayers();
}

public class PlayerService(
    IPlayerFactory playerFactory,
    IPlayerRepository playerRepository,
    ApplicationDbContext dbContext
) : IPlayerService
{
    public ValueTask<PlayerEntity?> GetPlayer(int playerId)
    {
        return playerRepository.GetByIdAsync(playerId);
    }

    public async Task<List<PlayerEntity>> GetPlayers()
    {
        return await playerRepository.GetAllAsync();
    }

    public async Task<PlayerEntity> GetOrCreatePlayerByName(string name)
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

    public async Task<List<PlayerEntity>> GetOrCreatePlayersByNames(ICollection<string> names)
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