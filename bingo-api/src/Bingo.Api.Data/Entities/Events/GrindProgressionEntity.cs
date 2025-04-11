using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities.Events;

[Serializable]
[Table("GrindProgressions")]
public class GrindProgressionEntity
{
    public int Id { get; set; }

    public required int GrindId { get; set; }
    public required GrindEntity Grind { get; set; }

    public bool IsGrindCompleted { get; set; }
    public List<ProgressionEntity> Progressions { get; set; } = [];
}