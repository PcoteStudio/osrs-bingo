using Bingo.Api.Core.Features.Boards.MultiLayer.Arguments;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Boards.MultiLayer;

public interface IMultiLayerBoardService
{
    Task<MultiLayerBoardEntity> CreateMultiLayerBoardAsync(int eventId, MultiLayerBoardCreateArguments args);
}

public class MultiLayerBoardService(
    IMultiLayerBoardFactory mlBoardFactory,
    IMultiLayerBoardUtil boardUtil,
    IMultiLayerBoardRepository mlBoardRepository,
    ApplicationDbContext dbContext
) : IMultiLayerBoardService
{
    public async Task<MultiLayerBoardEntity> CreateMultiLayerBoardAsync(int eventId,
        MultiLayerBoardCreateArguments args)
    {
        var mlBoard = mlBoardFactory.Create(eventId, args);
        boardUtil.CreateBoardLayers(mlBoard);
        mlBoardRepository.Add(mlBoard);
        await dbContext.SaveChangesAsync();
        return mlBoard;
    }
}