using AutoMapper;
using BingoBackend.Data;

namespace BingoBackend.Core.Features.Team;

public interface ITeamService
{
    Team CreateTeam(TeamCreateArguments arguments);
    Task<List<Team>> ListTeams();
}

public class TeamService(
    ITeamFactory teamFactory,
    ITeamRepository teamRepository,
    IMapper mapper,
    ApplicationDbContext dbContext
) : ITeamService
{
    public Team CreateTeam(TeamCreateArguments arguments)
    {
        var teamEntity = teamFactory.Create(arguments);
        teamRepository.Add(teamEntity);
        dbContext.SaveChanges();
        return mapper.Map<Team>(teamEntity);
    }

    public async Task<List<Team>> ListTeams()
    {
        var savedEntity = await teamRepository.GetAll();
        return savedEntity.Select(mapper.Map<Team>).ToList();
    }
}