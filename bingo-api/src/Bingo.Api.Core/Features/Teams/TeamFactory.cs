using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamFactory
{
    TeamEntity Create(int eventId, TeamCreateArguments args);
}

public class TeamFactory : ITeamFactory
{
    public TeamEntity Create(int eventId, TeamCreateArguments args)
    {
        return new TeamEntity
        {
            EventId = eventId,
            Name = args.Name
        };
    }
}