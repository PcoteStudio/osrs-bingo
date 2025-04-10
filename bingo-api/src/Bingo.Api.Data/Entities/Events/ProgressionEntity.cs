using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities.Events;

[Table("Progressions")]
public class ProgressionEntity
{
    public int Id { get; set; }

    public required int PlayerId { get; set; }
    public required PlayerEntity Player { get; set; }

    public required int GrindProgressionId { get; set; }
    public required GrindProgressionEntity GrindProgression { get; set; }

    public decimal? MetricsProgress { get; set; }
    public ItemEntity? Drop { get; set; }
}