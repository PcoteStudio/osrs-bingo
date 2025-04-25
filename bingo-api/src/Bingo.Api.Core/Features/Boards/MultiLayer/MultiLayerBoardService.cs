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
    IMultiLayerBoardRepository mlBoardRepository,
    ApplicationDbContext dbContext
) : IMultiLayerBoardService
{
    public async Task<MultiLayerBoardEntity> CreateMultiLayerBoardAsync(int eventId,
        MultiLayerBoardCreateArguments args)
    {
        var mlBoard = mlBoardFactory.Create(eventId, args);
        mlBoardRepository.Add(mlBoard);
        await dbContext.SaveChangesAsync();
        return mlBoard;
    }
}