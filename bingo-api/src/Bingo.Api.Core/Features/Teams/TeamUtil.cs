using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamUtil
{
    void UpdateTeam(TeamEntity team, TeamUpdateArguments arguments);
}

public class TeamUtil : ITeamUtil
{
    public void UpdateTeam(TeamEntity team, TeamUpdateArguments arguments)
    {
        team.Name = arguments.Name;
    }
}