namespace Bingo.Api.Web.Boards.MultiLayer;

[Serializable]
public class MultiLayerBoardShortResponse
{
    public int Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }
}