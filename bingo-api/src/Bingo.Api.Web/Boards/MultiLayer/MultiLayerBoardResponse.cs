using Bingo.Api.Web.Tiles;

namespace Bingo.Api.Web.Boards.MultiLayer;

[Serializable]
public class MultiLayerBoardResponse : MultiLayerBoardShortResponse
{
    public List<TileResponse> Tiles { get; set; } = [];
}