using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Teams;

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