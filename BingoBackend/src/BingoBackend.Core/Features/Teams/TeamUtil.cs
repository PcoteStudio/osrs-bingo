using BingoBackend.Core.Features.Teams.Arguments;
using BingoBackend.Data.Entities;

namespace BingoBackend.Core.Features.Teams;

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