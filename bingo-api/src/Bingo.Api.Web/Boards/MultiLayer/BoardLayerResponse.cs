using Bingo.Api.Web.Tiles;

namespace Bingo.Api.Web.Boards.MultiLayer;

[Serializable]
public class BoardLayerResponse : BoardLayerShortResponse
{
    public List<TileResponse> Tiles { get; set; } = [];
}