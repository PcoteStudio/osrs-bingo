using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BingoBackend.Data.Entities;

[Table("Events")]
public class EventEntity
{
    [Key] public int Id { get; set; }

    [MaxLength(255)] public string Name { get; set; } = string.Empty;

    // [ForeignKey("TeamId")] public List<TeamEntity> Teams { get; set; } = [];

    // [ForeignKey("PlayerId")] public List<PlayerEntity> Players { get; set; } = [];
}