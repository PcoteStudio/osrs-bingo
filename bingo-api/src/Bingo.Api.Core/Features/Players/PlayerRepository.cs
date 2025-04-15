using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerRepository : IGenericRepository<PlayerEntity>
{
    Task<List<PlayerEntity>> GetAllCompleteAsync();
    Task<PlayerEntity?> GetCompleteByIdAsync(int id);
    Task<PlayerEntity?> GetByNameAsync(string name);
    Task<List<PlayerEntity>> GetByNamesAsync(IEnumerable<string> playerNames);
}

public class PlayerRepository(ApplicationDbContext dbContext)
    : GenericRepository<PlayerEntity>(dbContext), IPlayerRepository
{
    public Task<List<PlayerEntity>> GetAllCompleteAsync()
    {
        return DbContext.Players
            .Include(p => p.Teams)
            .ToListAsync();
    }

    public Task<PlayerEntity?> GetCompleteByIdAsync(int id)
    {
        return DbContext.Players
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<PlayerEntity?> GetByNameAsync(string name)
    {
        return DbContext.Players
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public Task<List<PlayerEntity>> GetByNamesAsync(IEnumerable<string> playerNames)
    {
        return DbContext.Players
            .Where(p => playerNames.Contains(p.Name))
            .ToListAsync();
    }
}