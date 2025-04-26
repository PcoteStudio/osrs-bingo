namespace Bingo.Api.Web.Boards.MultiLayer;

[Serializable]
public class MultiLayerBoardResponse : MultiLayerBoardShortResponse
{
    public List<BoardLayerResponse> Layers { get; set; } = [];
}