using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("Progressions")]
public class ProgressionEntity
{
    private GrindProgressionEntity? _grindProgression;
    private PlayerEntity? _player;
    public int Id { get; set; }

    public required int PlayerId { get; set; }

    public PlayerEntity Player
    {
        get => _player.ThrowIfNotLoaded();
        set => _player = value;
    }

    public required int GrindProgressionId { get; set; }

    public GrindProgressionEntity GrindProgression
    {
        get => _grindProgression.ThrowIfNotLoaded();
        set => _grindProgression = value;
    }

    public double? MetricsProgress { get; set; }
    public ItemEntity? Drop { get; set; }
}