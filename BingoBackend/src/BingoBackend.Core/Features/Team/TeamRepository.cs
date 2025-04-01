using BingoBackend.Core.Features.Generic;
using BingoBackend.Data;
using BingoBackend.Data.Team;

namespace BingoBackend.Core.Features.Team;

public class TeamRepository(ApplicationDbContext context) : GenericRepository<TeamEntity>(context), ITeamRepository
{
    public IEnumerable<TeamEntity> GetRecentTeams(int count)
    {
        return Context.Teams.OrderByDescending(d => d.Id).Take(count).ToList();
    }
}