using AutoMapper;

namespace BingoBackend.Core.Features.Team;

public interface ITeamService
{
    Team CreateTeam(CreateTeamArguments arguments);
    Task<List<Team>> ListTeams();
}

public class TeamService(
    ITeamFactory teamFactory,
    ITeamRepository teamRepository,
    IMapper mapper
) : ITeamService
{
    public Team CreateTeam(CreateTeamArguments arguments)
    {
        var teamEntity = teamFactory.Create(arguments);
        teamRepository.Add(teamEntity);
        return mapper.Map<Team>(teamEntity);
    }

    public async Task<List<Team>> ListTeams()
    {
        var savedEntity = await teamRepository.GetAll();
        return savedEntity.Select(mapper.Map<Team>).ToList();
    }
}