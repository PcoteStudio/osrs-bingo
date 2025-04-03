using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BingoBackend.Data.Entities;

[Table("Teams")]
public class TeamEntity
{
    [Key] public int Id { get; set; }

    [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [ForeignKey("TeamId")] public List<PlayerEntity> Players { get; set; } = [];
}