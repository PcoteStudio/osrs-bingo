using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamRepository : IGenericRepository<TeamEntity>
{
    Task<TeamEntity?> GetCompleteByIdAsync(int id);
    Task<List<TeamEntity>> GetAllByEventIdAsync(int eventId);
}

public class TeamRepository(ApplicationDbContext dbContext) : GenericRepository<TeamEntity>(dbContext), ITeamRepository
{
    public Task<TeamEntity?> GetCompleteByIdAsync(int id)
    {
        return DbContext.Teams
            .Include(t => t.Event)
            .Include(t => t.Players)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public Task<List<TeamEntity>> GetAllByEventIdAsync(int eventId)
    {
        return DbContext.Teams
            .Where(t => t.EventId == eventId)
            .ToListAsync();
    }
}