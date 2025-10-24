using JetBrains.Annotations;

namespace Bingo.Api.Web.Boards.MultiLayer;

[PublicAPI]
public class BoardLayerShortResponse
{
    public int Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}