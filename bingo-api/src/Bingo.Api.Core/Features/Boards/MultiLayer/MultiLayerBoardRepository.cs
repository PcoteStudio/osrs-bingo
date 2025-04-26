using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Boards.MultiLayer;

public interface IMultiLayerBoardRepository : IGenericRepository<MultiLayerBoardEntity>
{
    Task<MultiLayerBoardEntity?> GetCompleteByIdAsync(int teamId);
}

public class MultiLayerBoardRepository(ApplicationDbContext dbContext)
    : GenericRepository<MultiLayerBoardEntity>(dbContext), IMultiLayerBoardRepository
{
    public Task<MultiLayerBoardEntity?> GetCompleteByIdAsync(int teamId)
    {
        return DbContext.MultiLayerBoards
            .Include(board => board.Event)
            .Include(board => board.Layers)
            .ThenInclude(boardLayer => boardLayer.Tiles)
            .ThenInclude(tiles => tiles.GrindProgressions)
            .ThenInclude(gp => gp.Progressions)
            .ThenInclude(progressions => progressions.Drop)
            .Include(board => board.Layers)
            .ThenInclude(boardLayer => boardLayer.Tiles)
            .ThenInclude(tiles => tiles.GrindProgressions)
            .ThenInclude(gp => gp.Progressions)
            .ThenInclude(progressions => progressions.Player.Teams)
            .Include(board => board.Layers)
            .ThenInclude(boardLayer => boardLayer.Tiles)
            .ThenInclude(tiles => tiles.GrindProgressions)
            .ThenInclude(gp => gp.Grind.Drops)
            .FirstOrDefaultAsync(t => t.Id == teamId);
    }
}