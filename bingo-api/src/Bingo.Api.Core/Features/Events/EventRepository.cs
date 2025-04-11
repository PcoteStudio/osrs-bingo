using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Events;

public interface IEventRepository : IGenericRepository<EventEntity>
{
    Task<EventEntity?> GetCompleteByIdAsync(int eventId);
}

public class EventRepository(ApplicationDbContext dbContext)
    : GenericRepository<EventEntity>(dbContext), IEventRepository
{
    public Task<EventEntity?> GetCompleteByIdAsync(int eventId)
    {
        return DbContext.Events
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == eventId);
    }
}