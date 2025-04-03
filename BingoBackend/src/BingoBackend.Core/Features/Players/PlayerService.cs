using AutoMapper;
using BingoBackend.Data;

namespace BingoBackend.Core.Features.Players;

public interface IPlayerService
{
    Task<Player?> GetPlayer(int playerId);
    Task<Player> GetOrCreatePlayerByName(string name);
    Task<List<Player>> GetOrCreatePlayersByNames(ICollection<string> names);
    Task<List<Player>> ListPlayers();
}

public class PlayerService(
    IPlayerFactory playerFactory,
    IPlayerRepository playerRepository,
    IMapper mapper,
    ApplicationDbContext dbContext
) : IPlayerService
{
    public async Task<Player?> GetPlayer(int playerId)
    {
        var playerEntity = await playerRepository.GetByIdAsync(playerId);
        return mapper.Map<Player>(playerEntity);
    }

    public async Task<List<Player>> ListPlayers()
    {
        var playerEntities = await playerRepository.GetAllAsync();
        return playerEntities.Select(mapper.Map<Player>).ToList();
    }

    public async Task<Player> GetOrCreatePlayerByName(string name)
    {
        var playerEntity = await playerRepository.GetByNameAsync(name);
        if (playerEntity is null)
        {
            playerEntity = playerFactory.Create(name);
            playerRepository.Add(playerEntity);
            await dbContext.SaveChangesAsync();
        }

        return mapper.Map<Player>(playerEntity);
    }

    public async Task<List<Player>> GetOrCreatePlayersByNames(ICollection<string> names)
    {
        var foundPlayerEntities = await playerRepository.GetByNamesAsync(names);
        var namesNotFound = names.Except(foundPlayerEntities.Select(x => x.Name));
        var newPlayerEntities = namesNotFound.Select(playerFactory.Create).ToList();
        if (newPlayerEntities.Count > 0)
        {
            playerRepository.Add(newPlayerEntities);
            await dbContext.SaveChangesAsync();
        }

        var playerEntities = foundPlayerEntities.Concat(newPlayerEntities);
        return playerEntities.Select(mapper.Map<Player>).ToList();
    }
}