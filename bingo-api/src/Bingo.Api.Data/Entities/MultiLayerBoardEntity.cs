using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("MultiLayerBoards")]
public class MultiLayerBoardEntity : BoardEntity
{
    private List<BoardLayerEntity>? _layers;
    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }

    [ForeignKey("BoardId")]
    public List<BoardLayerEntity> Layers
    {
        get => _layers.ThrowIfNotLoaded();
        set => _layers = value;
    }
}