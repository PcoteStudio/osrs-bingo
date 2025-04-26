using Bingo.Api.Core.Features.Boards.MultiLayer.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Boards.MultiLayer;

public interface IMultiLayerBoardUtil
{
    void CreateBoardLayers(MultiLayerBoardEntity board);
}

public class MultiLayerBoardUtil(IBoardLayerFactory boardLayerFactory) : IMultiLayerBoardUtil
{
    public void CreateBoardLayers(MultiLayerBoardEntity board)
    {
        var layerArgs = new BoardLayerCreateArguments
        {
            Height = board.Height,
            Width = board.Width
        };
        for (var i = board.Layers.Count; i < board.Depth; i++) board.Layers.Add(boardLayerFactory.Create(layerArgs));
    }
}