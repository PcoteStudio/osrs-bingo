using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamRepository : IGenericRepository<TeamEntity>
{
    Task<TeamEntity?> GetCompleteByIdAsync(int teamId);
    Task<List<TeamEntity>> GetAllByEventIdAsync(int eventId);
    Task<List<TeamEntity>> GetAllByIdsAsync(ICollection<int> teamIds);
}

public class TeamRepository(ApplicationDbContext dbContext) : GenericRepository<TeamEntity>(dbContext), ITeamRepository
{
    public Task<TeamEntity?> GetCompleteByIdAsync(int teamId)
    {
        return DbContext.Teams
            .Include(t => t.Event)
            .ThenInclude(e => e.Administrators)
            .Include(t => t.Players)
            .FirstOrDefaultAsync(t => t.Id == teamId);
    }

    public Task<List<TeamEntity>> GetAllByEventIdAsync(int eventId)
    {
        return DbContext.Teams
            .Where(t => t.EventId == eventId)
            .ToListAsync();
    }

    public Task<List<TeamEntity>> GetAllByIdsAsync(ICollection<int> teamIds)
    {
        return DbContext.Teams
            .Where(t => teamIds.Contains(t.Id))
            .ToListAsync();
    }
}