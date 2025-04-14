using Bingo.Api.Core.Features.Players.Arguments;
using Bingo.Api.Core.Features.Players.Exceptions;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerService
{
    Task<PlayerEntity> CreatePlayerAsync(PlayerCreateArguments args);
    Task<PlayerEntity> GetOrCreatePlayerByNameAsync(string name);
    Task<List<PlayerEntity>> GetOrCreatePlayersByNamesAsync(ICollection<string> names);
    Task<List<PlayerEntity>> GetPlayersAsync();
    Task<PlayerEntity> RemovePlayerAsync(int teamId);
    Task<PlayerEntity> UpdatePlayerAsync(int playerId, PlayerUpdateArguments args);
}

public class PlayerService(
    IPlayerFactory playerFactory,
    IPlayerRepository playerRepository,
    IPlayerUtil playerUtil,
    IPlayerServiceHelper playerServiceHelper,
    ITeamServiceHelper teamServiceHelper,
    ApplicationDbContext dbContext
) : IPlayerService
{
    public async Task<PlayerEntity> CreatePlayerAsync(PlayerCreateArguments args)
    {
        await playerServiceHelper.EnsurePlayerDoesNotExistByNameAsync(args.Name);
        var player = playerFactory.Create(args);
        var teams = await teamServiceHelper.GetAllRequiredByIdsAsync(args.TeamIds);
        player.Teams.AddRange(teams);
        playerRepository.Add(player);
        await dbContext.SaveChangesAsync();
        return player;
    }

    public async Task<PlayerEntity> GetOrCreatePlayerByNameAsync(string name)
    {
        var player = await playerRepository.GetByNameAsync(name);
        if (player is null)
        {
            player = playerFactory.Create(name);
            playerRepository.Add(player);
            await dbContext.SaveChangesAsync();
        }

        return player;
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

    public async Task<List<PlayerEntity>> GetPlayersAsync()
    {
        return await playerRepository.GetAllAsync();
    }

    public virtual async Task<PlayerEntity> RemovePlayerAsync(int teamId)
    {
        var player = await playerRepository.GetCompleteByIdAsync(teamId);
        if (player == null) throw new PlayerNotFoundException(teamId);
        playerRepository.Remove(player);
        return player;
    }

    public async Task<PlayerEntity> UpdatePlayerAsync(int playerId, PlayerUpdateArguments args)
    {
        var playerEntity = await playerServiceHelper.GetRequiredCompletePlayerAsync(playerId);
        playerUtil.UpdatePlayer(playerEntity, args);
        var teams = await teamServiceHelper.GetAllRequiredByIdsAsync(args.TeamIds);
        playerEntity.Teams = teams;
        dbContext.Update(playerEntity);
        await dbContext.SaveChangesAsync();
        return playerEntity;
    }
}