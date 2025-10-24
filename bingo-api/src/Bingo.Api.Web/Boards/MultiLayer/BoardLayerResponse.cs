using Bingo.Api.Web.Tiles;
using JetBrains.Annotations;

namespace Bingo.Api.Web.Boards.MultiLayer;

[PublicAPI]
public class BoardLayerResponse : BoardLayerShortResponse
{
    public List<TileResponse> Tiles { get; set; } = [];
}