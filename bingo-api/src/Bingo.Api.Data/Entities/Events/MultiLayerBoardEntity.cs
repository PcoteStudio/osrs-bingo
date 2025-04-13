using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities.Events;

[Serializable]
[Table("MultiLayerBoards")]
public class MultiLayerBoardEntity : BoardEntity
{
    private List<TileEntity>? _tiles;
    public int Width { get; set; }
    public int Height { get; set; }

    [ForeignKey("MultiLayerBoardId")]
    public List<TileEntity> Tiles
    {
        get => _tiles.ThrowIfNotLoaded();
        set => _tiles = value;
    }
}