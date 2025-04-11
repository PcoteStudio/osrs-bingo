using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerRepository : IGenericRepository<PlayerEntity>
{
    Task<PlayerEntity?> GetByNameAsync(string name);
    Task<List<PlayerEntity>> GetByNamesAsync(IEnumerable<string> playerNames);
}

public class PlayerRepository(ApplicationDbContext dbContext)
    : GenericRepository<PlayerEntity>(dbContext), IPlayerRepository
{
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

    public Task<PlayerEntity?> GetCompletePlayerByIdAsync(int playerId)
    {
        return DbContext.Players
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == playerId);
    }
}