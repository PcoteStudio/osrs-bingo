using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities.Events;

[Serializable]
[Table("GrindProgressions")]
public class GrindProgressionEntity
{
    private GrindEntity? _grind;
    private List<ProgressionEntity>? _progressions;

    public int Id { get; set; }

    public required int GrindId { get; set; }

    public GrindEntity Grind
    {
        get => _grind.ThrowIfNotLoaded();
        set => _grind = value;
    }

    public bool IsGrindCompleted { get; set; }

    public List<ProgressionEntity> Progressions
    {
        get => _progressions.ThrowIfNotLoaded();
        set => _progressions = value;
    }
}