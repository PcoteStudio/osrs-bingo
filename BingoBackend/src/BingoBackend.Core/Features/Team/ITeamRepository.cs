using BingoBackend.Core.Features.Generic;
using BingoBackend.Data.Team;

namespace BingoBackend.Core.Features.Team;

public interface ITeamRepository : IGenericRepository<TeamEntity>
{
    IEnumerable<TeamEntity> GetRecentTeams(int count);
}