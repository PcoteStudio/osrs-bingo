using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamUtil
{
    void UpdateTeam(TeamEntity team, TeamUpdateArguments args);
}

public class TeamUtil : ITeamUtil
{
    public void UpdateTeam(TeamEntity team, TeamUpdateArguments args)
    {
        team.Name = args.Name;
    }
}