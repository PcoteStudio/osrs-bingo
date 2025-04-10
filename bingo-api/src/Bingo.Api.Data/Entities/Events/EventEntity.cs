using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities.Events;

[Table("Events")]
public class EventEntity
{
    public int Id { get; set; }

    [MaxLength(255)] public string Name { get; set; } = string.Empty;

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset EndTime { get; set; }

    [ForeignKey("EventId")] public List<TeamEntity> Teams { get; set; } = [];
}