using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Items;

public interface IItemRepository : IGenericRepository<ItemEntity>
{
    Task<List<ItemEntity>> GetAllCompleteAsync();
    Task<PlayerEntity?> GetByNameAsync(string name);
    Task<ItemEntity?> GetCompleteByIdAsync(int id);
}

public class ItemRepository(ApplicationDbContext dbContext)
    : GenericRepository<ItemEntity>(dbContext), IItemRepository
{
    public Task<List<ItemEntity>> GetAllCompleteAsync()
    {
        return DbContext.Items
            .Include(n => n.DropInfos)
            .ToListAsync();
    }

    public Task<PlayerEntity?> GetByNameAsync(string name)
    {
        return DbContext.Players
            .FirstOrDefaultAsync(n => n.Name == name);
    }

    public Task<ItemEntity?> GetCompleteByIdAsync(int id)
    {
        return DbContext.Items
            .Include(n => n.DropInfos)
            .FirstOrDefaultAsync(n => n.Id == id);
    }
}