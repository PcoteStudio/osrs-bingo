using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Tiles;

public interface ITileUtil
{
    void UpdateIsCompleted(TileEntity tile);
}

public class TileUtil : ITileUtil
{
    public void UpdateIsCompleted(TileEntity tile)
    {
        tile.IsCompleted = tile.GrindProgressions.Count(gp => gp.IsGrindCompleted) >= tile.GrindCountForCompletion;
    }
}