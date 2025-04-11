using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamService
{
    Task<TeamEntity> CreateTeamAsync(int eventId, TeamCreateArguments args);
    Task<List<TeamEntity>> GetEventTeamsAsync(int eventId);
    Task<TeamEntity> GetRequiredTeamAsync(int teamId);
    Task<TeamEntity> UpdateTeamAsync(int teamId, TeamUpdateArguments args);
    Task<TeamEntity> AddTeamPlayersAsync(int teamId, TeamPlayersArguments args);
    Task<TeamEntity> RemoveTeamPlayerAsync(int teamId, string playerName);
}

public class TeamService(
    ITeamFactory teamFactory,
    ITeamRepository teamRepository,
    ITeamUtil teamUtil,
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

    public async Task<TeamEntity> GetRequiredTeamAsync(int teamId)
    {
        var team = await teamRepository.GetCompleteByIdAsync(teamId);
        if (team == null) throw new TeamNotFoundException(teamId);
        return team;
    }

    public Task<List<TeamEntity>> GetEventTeamsAsync(int eventId)
    {
        return teamRepository.GetAllByEventIdAsync(eventId);
    }

    public async Task<TeamEntity> UpdateTeamAsync(int teamId, TeamUpdateArguments args)
    {
        var team = await GetRequiredTeamAsync(teamId);
        teamUtil.UpdateTeam(team, args);
        dbContext.Update(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> AddTeamPlayersAsync(int teamId, TeamPlayersArguments args)
    {
        var team = await GetRequiredTeamAsync(teamId);
        var players = await playerService.GetOrCreatePlayersByNamesAsync(args.PlayerNames);
        var newPlayers = players
            .Where(newPlayer => team.Players
                .All(teamPlayer => newPlayer.Id != teamPlayer.Id));
        team.Players.AddRange(newPlayers);
        dbContext.Update(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> RemoveTeamPlayerAsync(int teamId, string playerName)
    {
        var team = await GetRequiredTeamAsync(teamId);
        var player = await playerService.GetOrCreatePlayerByNameAsync(playerName);
        if (team.Players.All(p => p.Id != player.Id))
        {
            team.Players.Add(player);
            dbContext.Update(team);
            await dbContext.SaveChangesAsync();
        }

        return team;
    }
}