using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerRepository : IGenericRepository<PlayerEntity>
{
    Task<PlayerEntity?> GetByNameAsync(string name);
    Task<List<PlayerEntity>> GetByNamesAsync(IEnumerable<string> playerNames);
}

public class PlayerRepository(ApplicationDbContext context)
    : GenericRepository<PlayerEntity>(context), IPlayerRepository
{
    public Task<PlayerEntity?> GetByNameAsync(string name)
    {
        return Context.Players
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public Task<List<PlayerEntity>> GetByNamesAsync(IEnumerable<string> playerNames)
    {
        return Context.Players
            .Where(p => playerNames.Contains(p.Name))
            .ToListAsync();
    }

    public Task<PlayerEntity?> GetCompletePlayerById(int playerId)
    {
        return Context.Players
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == playerId);
    }
}