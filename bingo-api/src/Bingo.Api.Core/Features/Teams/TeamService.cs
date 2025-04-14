using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamService
{
    Task<TeamEntity> AddTeamPlayersAsync(int teamId, TeamPlayersArguments args);
    Task<TeamEntity> CreateTeamAsync(int eventId, TeamCreateArguments args);
    Task<TeamEntity> UpdateTeamAsync(int teamId, TeamUpdateArguments args);
    Task<TeamEntity> RemoveTeamPlayerAsync(int teamId, string playerName);
    Task<TeamEntity> UpdateTeamPlayersAsync(int teamId, TeamPlayersArguments args);
}

public class TeamService(
    ITeamFactory teamFactory,
    ITeamRepository teamRepository,
    ITeamUtil teamUtil,
    ITeamServiceHelper teamServiceHelper,
    IPlayerService playerService,
    ApplicationDbContext dbContext
) : ITeamService
{
    public async Task<TeamEntity> CreateTeamAsync(int eventId, TeamCreateArguments args)
    {
        var team = teamFactory.Create(eventId, args);
        teamRepository.Add(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> UpdateTeamAsync(int teamId, TeamUpdateArguments args)
    {
        var team = await teamServiceHelper.GetRequiredCompleteTeamAsync(teamId);
        teamUtil.UpdateTeam(team, args);
        dbContext.Update(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> AddTeamPlayersAsync(int teamId, TeamPlayersArguments args)
    {
        var team = await teamServiceHelper.GetRequiredCompleteTeamAsync(teamId);
        var players = await playerService.GetOrCreatePlayersByNamesAsync(args.PlayerNames);
        var newPlayers = players
            .Where(newPlayer => team.Players
                .All(teamPlayer => newPlayer.Id != teamPlayer.Id));
        team.Players.AddRange(newPlayers);
        dbContext.Update(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> UpdateTeamPlayersAsync(int teamId, TeamPlayersArguments args)
    {
        var team = await teamServiceHelper.GetRequiredCompleteTeamAsync(teamId);
        var players = await playerService.GetOrCreatePlayersByNamesAsync(args.PlayerNames);
        team.Players = players;
        dbContext.Update(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> RemoveTeamPlayerAsync(int teamId, string playerName)
    {
        var team = await teamServiceHelper.GetRequiredCompleteTeamAsync(teamId);
        var player = await playerService.GetOrCreatePlayerByNameAsync(playerName);
        if (team.Players.Contains(player))
        {
            team.Players.Remove(player);
            dbContext.Update(team);
            await dbContext.SaveChangesAsync();
        }

        return team;
    }
}