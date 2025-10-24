using JetBrains.Annotations;

namespace Bingo.Api.Web.Boards.MultiLayer;

[PublicAPI]
public class MultiLayerBoardResponse : MultiLayerBoardShortResponse
{
    public List<BoardLayerResponse> Layers { get; set; } = [];
}