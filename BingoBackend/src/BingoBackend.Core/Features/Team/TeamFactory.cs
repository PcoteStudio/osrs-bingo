using BingoBackend.Data.Team;

namespace BingoBackend.Core.Features.Team;

public interface ITeamFactory
{
    TeamEntity Create(TeamCreateArguments arguments);
}

public class TeamFactory : ITeamFactory
{
    public TeamEntity Create(TeamCreateArguments arguments)
    {
        return new TeamEntity
        {
            Name = arguments.Name
        };
    }
}