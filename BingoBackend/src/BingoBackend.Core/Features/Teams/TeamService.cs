using BingoBackend.Core.Features.Players;
using BingoBackend.Core.Features.Teams.Arguments;
using BingoBackend.Core.Features.Teams.Exceptions;
using BingoBackend.Data;
using BingoBackend.Data.Entities;

namespace BingoBackend.Core.Features.Teams;

public interface ITeamService
{
    Task<TeamEntity> CreateTeamAsync(TeamCreateArguments args);
    Task<List<TeamEntity>> GetTeamsAsync();
    Task<TeamEntity> GetTeamAsync(int teamId);
    Task<TeamEntity> UpdateTeamAsync(int teamId, TeamUpdateArguments args);
    Task<TeamEntity> AddTeamPlayers(int teamId, TeamPlayersArguments args);
}

public class TeamService(
    ITeamFactory teamFactory,
    ITeamRepository teamRepository,
    ITeamUtil teamUtil,
    IPlayerService playerService,
    ApplicationDbContext dbContext
) : ITeamService
{
    public async Task<TeamEntity> CreateTeamAsync(TeamCreateArguments args)
    {
        var team = teamFactory.Create(args);
        teamRepository.Add(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public Task<List<TeamEntity>> GetTeamsAsync()
    {
        return teamRepository.GetAllAsync();
    }

    public async Task<TeamEntity> GetTeamAsync(int teamId)
    {
        var team = await teamRepository.GetCompleteByIdAsync(teamId);
        if (team == null) throw new TeamNotFoundException(teamId);
        return team;
    }

    public async Task<TeamEntity> UpdateTeamAsync(int teamId, TeamUpdateArguments args)
    {
        var team = await teamRepository.GetCompleteByIdAsync(teamId);
        if (team == null) throw new TeamNotFoundException(teamId);
        teamUtil.UpdateTeam(team, args);
        dbContext.Update(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> AddTeamPlayers(int teamId, TeamPlayersArguments args)
    {
        var team = await teamRepository.GetCompleteByIdAsync(teamId);
        if (team == null) throw new TeamNotFoundException(teamId);
        var players = await playerService.GetOrCreatePlayersByNames(args.PlayerNames);
        var newPlayers = players
            .Where(newPlayer => team.Players
                .All(teamPlayer => newPlayer.Id != teamPlayer.Id));
        team.Players.AddRange(newPlayers);
        return team;
    }
}