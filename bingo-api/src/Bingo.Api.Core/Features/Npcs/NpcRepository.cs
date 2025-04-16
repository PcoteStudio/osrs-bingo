using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Npcs;

public interface INpcRepository : IGenericRepository<NpcEntity>
{
    Task<List<NpcEntity>> GetAllCompleteAsync();
    Task<PlayerEntity?> GetByNameAsync(string name);
    Task<NpcEntity?> GetCompleteByIdAsync(int id);
}

public class NpcRepository(ApplicationDbContext dbContext)
    : GenericRepository<NpcEntity>(dbContext), INpcRepository
{
    public Task<List<NpcEntity>> GetAllCompleteAsync()
    {
        return DbContext.Npcs
            .Include(n => n.Drops)
            .ToListAsync();
    }

    public Task<PlayerEntity?> GetByNameAsync(string name)
    {
        return DbContext.Players
            .FirstOrDefaultAsync(n => n.Name == name);
    }

    public Task<NpcEntity?> GetCompleteByIdAsync(int id)
    {
        return DbContext.Npcs
            .Include(n => n.Drops)
            .FirstOrDefaultAsync(n => n.Id == id);
    }
}