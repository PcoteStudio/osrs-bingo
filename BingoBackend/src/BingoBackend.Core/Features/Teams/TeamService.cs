using AutoMapper;
using BingoBackend.Core.Features.Players;
using BingoBackend.Data;

namespace BingoBackend.Core.Features.Teams;

public interface ITeamService
{
    Team CreateTeam(TeamCreateArguments args);
    Task<List<Team>> ListTeamsAsync();
    Task<Team?> GetTeamAsync(int teamId);
    Task<Team> UpdateTeam(TeamCreateArguments args);
}

public class TeamService(
    ITeamFactory teamFactory,
    ITeamRepository teamRepository,
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

    public async Task<Team?> GetTeamAsync(int teamId)
    {
        var teamEntity = await teamRepository.GetByIdAsync(teamId);
        return mapper.Map<Team>(teamEntity);
    }

    public Task<Team> UpdateTeam(TeamCreateArguments args)
    {
        throw new NotImplementedException();
    }
}