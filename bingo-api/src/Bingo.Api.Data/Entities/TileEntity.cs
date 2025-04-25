using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("Tiles")]
public class TileEntity
{
    private BoardEntity? _board;
    private EventEntity? _event;
    private List<GrindProgressionEntity>? _grindProgressions;
    [Key] public int Id { get; set; }
    [MaxLength(255)] public string Name { get; set; } = string.Empty;
    [MaxLength(255)] public string Description { get; set; } = string.Empty;
    public int GrindCountForCompletion { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsActive { get; set; }
    public int EventId { get; set; }

    public EventEntity Event
    {
        get => _event.ThrowIfNotLoaded();
        set => _event = value;
    }

    public int BoardId { get; set; }

    public BoardEntity Board
    {
        get => _board.ThrowIfNotLoaded();
        set => _board = value;
    }

    public List<GrindProgressionEntity> GrindProgressions
    {
        get => _grindProgressions.ThrowIfNotLoaded();
        set => _grindProgressions = value;
    }
}