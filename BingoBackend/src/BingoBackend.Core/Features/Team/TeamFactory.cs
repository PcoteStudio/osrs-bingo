using BingoBackend.Data.Team;

namespace BingoBackend.Core.Features.Team;

public interface ITeamFactory
{
    TeamEntity Create(CreateTeamArguments arguments);
}

public class TeamFactory : ITeamFactory
{
    public TeamEntity Create(CreateTeamArguments arguments)
    {
        return new TeamEntity
        {
            Name = arguments.Name
        };
    }
}