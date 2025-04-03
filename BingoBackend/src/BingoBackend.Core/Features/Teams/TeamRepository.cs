using BingoBackend.Core.Features.Generic;
using BingoBackend.Data;
using BingoBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BingoBackend.Core.Features.Teams;

public interface ITeamRepository : IGenericRepository<TeamEntity>
{
    Task<TeamEntity?> GetCompleteByIdAsync(int id);
}

public class TeamRepository(ApplicationDbContext context) : GenericRepository<TeamEntity>(context), ITeamRepository
{
    public Task<TeamEntity?> GetCompleteByIdAsync(int id)
    {
        return Context.Set<TeamEntity>()
            .Include(t => t.Players)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}