namespace BingoBackend.Core.Features.Team;

public class TeamService
{
    public Team CreateTeam(string name)
    {
        return new Team { Name = name };
    }
}