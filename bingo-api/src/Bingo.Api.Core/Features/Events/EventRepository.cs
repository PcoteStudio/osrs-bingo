using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Events;

public interface IEventRepository : IGenericRepository<EventEntity>
{
    Task<EventEntity?> GetCompleteByIdAsync(int eventId);
    new Task<List<EventEntity>> GetAllAsync();
}

public class EventRepository(ApplicationDbContext dbContext)
    : GenericRepository<EventEntity>(dbContext), IEventRepository
{
    public Task<EventEntity?> GetCompleteByIdAsync(int eventId)
    {
        return DbContext.Events
            .Include(e => e.Administrators)
            .Include(e => e.Teams)
            .ThenInclude(t => t.Players)
            .FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public new Task<List<EventEntity>> GetAllAsync()
    {
        return DbContext.Events
            .Include(e => e.Administrators)
            .Include(e => e.Teams)
            .ThenInclude(t => t.Players)
            .ToListAsync();
    }
}