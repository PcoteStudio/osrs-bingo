using AutoMapper;
using BingoBackend.Core.Features.Players;
using BingoBackend.Core.Features.Teams.Arguments;
using BingoBackend.Core.Features.Teams.Exceptions;
using BingoBackend.Data;

namespace BingoBackend.Core.Features.Teams;

public interface ITeamService
{
    Team CreateTeam(TeamCreateArguments args);
    Task<List<Team>> ListTeamsAsync();
    Task<Team> GetTeamAsync(int teamId);
    Task<Team> UpdateTeamAsync(int teamId, TeamUpdateArguments args);
    Task<Team> AddTeamPlayers(int teamId, TeamPlayersArguments args);
}

public class TeamService(
    ITeamFactory teamFactory,
    ITeamRepository teamRepository,
    ITeamUtil teamUtil,
    IPlayerService playerService,
    IMapper mapper,
    ApplicationDbContext dbContext
) : ITeamService
{
    public Team CreateTeam(TeamCreateArguments args)
    {
        var teamEntity = teamFactory.Create(args);
        teamRepository.Add(teamEntity);
        dbContext.SaveChanges();
        return mapper.Map<Team>(teamEntity);
    }

    public async Task<List<Team>> ListTeamsAsync()
    {
        var savedEntity = await teamRepository.GetAllAsync();
        return savedEntity.Select(mapper.Map<Team>).ToList();
    }

    public async Task<Team> GetTeamAsync(int teamId)
    {
        var teamEntity = await teamRepository.GetCompleteByIdAsync(teamId);
        if (teamEntity == null) throw new TeamNotFoundException(teamId);
        return mapper.Map<Team>(teamEntity);
    }

    public async Task<Team> UpdateTeamAsync(int teamId, TeamUpdateArguments args)
    {
        var teamEntity = await teamRepository.GetCompleteByIdAsync(teamId);
        if (teamEntity == null) throw new TeamNotFoundException(teamId);
        teamUtil.UpdateTeamEntity(teamEntity, args);
        dbContext.Update(teamEntity);
        await dbContext.SaveChangesAsync();
        return mapper.Map<Team>(teamEntity);
    }

    public async Task<Team> AddTeamPlayers(int teamId, TeamPlayersArguments args)
    {
        var teamEntity = await teamRepository.GetCompleteByIdAsync(teamId);
        if (teamEntity == null) throw new TeamNotFoundException(teamId);
        var players = await playerService.GetOrCreatePlayersByNames(args.PlayerNames);
        // TODO Add players to team and save
        return mapper.Map<Team>(teamEntity);
    }
}