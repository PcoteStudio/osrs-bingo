using Bingo.Api.Core.Features.Boards.MultiLayer.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Boards.MultiLayer;

public interface IBoardLayerFactory
{
    BoardLayerEntity Create(BoardLayerCreateArguments args);
}

public class BoardLayerFactory : IBoardLayerFactory
{
    public BoardLayerEntity Create(BoardLayerCreateArguments args)
    {
        return new BoardLayerEntity
        {
            Width = args.Width,
            Height = args.Height,
            Tiles = []
        };
    }
}