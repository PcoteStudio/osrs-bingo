using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Drops;

public interface IDropRepository : IGenericRepository<DropEntity>
{
    Task<List<DropEntity>> GetAllCompleteAsync();
    Task<DropEntity?> GetByNpcIdAndItemIdAsync(int npcId, int itemId);
    Task<DropEntity?> GetCompleteByIdAsync(int id);
}

public class DropRepository(ApplicationDbContext dbContext)
    : GenericRepository<DropEntity>(dbContext), IDropRepository
{
    public Task<List<DropEntity>> GetAllCompleteAsync()
    {
        return DbContext.Drops
            .Include(n => n.Item)
            .Include(n => n.Npc)
            .ToListAsync();
    }

    public Task<DropEntity?> GetByNpcIdAndItemIdAsync(int npcId, int itemId)
    {
        return DbContext.Drops.FirstOrDefaultAsync(n => n.NpcId == npcId && n.ItemId == itemId);
    }

    public Task<DropEntity?> GetCompleteByIdAsync(int id)
    {
        return DbContext.Drops
            .Include(n => n.Item)
            .Include(n => n.Npc)
            .FirstOrDefaultAsync(n => n.Id == id);
    }
}