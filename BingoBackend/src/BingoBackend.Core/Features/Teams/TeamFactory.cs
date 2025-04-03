using BingoBackend.Data.Entities;

namespace BingoBackend.Core.Features.Teams;

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