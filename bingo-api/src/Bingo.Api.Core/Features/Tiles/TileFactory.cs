using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Tiles;

public interface ITileFactory
{
    TileEntity Create(int eventId, int boardId);
}

public class TileFactory : ITileFactory
{
    public TileEntity Create(int eventId, int boardId)
    {
        return new TileEntity
        {
            EventId = eventId,
            BoardId = boardId,
            GrindProgressions = []
        };
    }
}