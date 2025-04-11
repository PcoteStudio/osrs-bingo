using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities.Events;

[Serializable]
[Table("MultiLayerBoards")]
public class MultiLayerBoardEntity : BoardEntity
{
    public int Width { get; set; }
    public int Height { get; set; }
    [ForeignKey("MultiLayerBoardId")] public List<TileEntity> Tiles { get; set; } = [];
}