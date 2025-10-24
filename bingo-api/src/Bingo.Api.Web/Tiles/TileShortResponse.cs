using JetBrains.Annotations;

namespace Bingo.Api.Web.Tiles;

[PublicAPI]
public class TileShortResponse
{
    public int EventId { get; set; }
    public int BoardId { get; set; }
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int GrindCountForCompletion { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsActive { get; set; }
}