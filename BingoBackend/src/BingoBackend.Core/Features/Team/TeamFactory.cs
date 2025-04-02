using BingoBackend.Data.Team;

namespace BingoBackend.Core.Features.Team;

public interface ITeamFactory
{
    TeamEntity Create(TeamCreateArguments args);
}

public class TeamFactory : ITeamFactory
{
    public TeamEntity Create(TeamCreateArguments args)
    {
        return new TeamEntity
        {
            Name = args.Name
        };
    }
}