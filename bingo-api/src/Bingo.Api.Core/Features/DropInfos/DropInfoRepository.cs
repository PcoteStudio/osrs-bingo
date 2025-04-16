using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.DropInfos;

public interface IDropInfoRepository : IGenericRepository<DropInfoEntity>
{
    Task<List<DropInfoEntity>> GetAllCompleteAsync();
    Task<DropInfoEntity?> GetByNpcIdAndItemIdAsync(int npcId, int itemId);
    Task<DropInfoEntity?> GetCompleteByIdAsync(int id);
}

public class DropInfoRepository(ApplicationDbContext dbContext)
    : GenericRepository<DropInfoEntity>(dbContext), IDropInfoRepository
{
    public Task<List<DropInfoEntity>> GetAllCompleteAsync()
    {
        return DbContext.DropInfos
            .Include(n => n.Item)
            .Include(n => n.Npc)
            .ToListAsync();
    }

    public Task<DropInfoEntity?> GetByNpcIdAndItemIdAsync(int npcId, int itemId)
    {
        return DbContext.DropInfos.FirstOrDefaultAsync(n => n.NpcId == npcId && n.ItemId == itemId);
    }

    public Task<DropInfoEntity?> GetCompleteByIdAsync(int id)
    {
        return DbContext.DropInfos
            .Include(n => n.Item)
            .Include(n => n.Npc)
            .FirstOrDefaultAsync(n => n.Id == id);
    }
}