using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("BoardLayers")]
public class BoardLayerEntity
{
    private MultiLayerBoardEntity? _board;
    private List<TileEntity>? _tiles;
    public int Id { get; set; }
    public int BoardId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    [ForeignKey("BoardLayerId")]
    public List<TileEntity> Tiles
    {
        get => _tiles.ThrowIfNotLoaded();
        set => _tiles = value;
    }

    public MultiLayerBoardEntity Board
    {
        get => _board.ThrowIfNotLoaded();
        set => _board = value;
    }
}