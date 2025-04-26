using Bingo.Api.Core.Features.Boards.MultiLayer.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Boards.MultiLayer;

public interface IMultiLayerBoardFactory
{
    MultiLayerBoardEntity Create(int eventId, MultiLayerBoardCreateArguments args);
}

public class MultiLayerBoardFactory : IMultiLayerBoardFactory
{
    public MultiLayerBoardEntity Create(int eventId, MultiLayerBoardCreateArguments args)
    {
        return new MultiLayerBoardEntity
        {
            EventId = eventId,
            Width = args.Width,
            Height = args.Height,
            Depth = args.Depth,
            Layers = []
        };
    }
}