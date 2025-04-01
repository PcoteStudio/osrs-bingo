using AutoMapper;
using BingoBackend.Data.Team;

namespace BingoBackend.Core.Features.Team;

public class TeamService(ITeamRepository teamRepository)
{
    // TODO Move Automapper to dependency injection
    private static readonly MapperConfiguration configFromEntity = new(cfg => cfg.CreateMap<TeamEntity, Team>());
    private static readonly Mapper mapperFromEntity = new(configFromEntity);

    public Team CreateTeam(string name)
    {
        var teamEntity = new TeamEntity { Name = name };
        var savedEntity = teamRepository.Add(teamEntity);
        return mapperFromEntity.Map<Team>(savedEntity);
    }

    public IEnumerable<Team> ListTeams()
    {
        var savedEntity = teamRepository.GetAll();
        return savedEntity.Select(x => mapperFromEntity.Map<Team>(x));
    }
}