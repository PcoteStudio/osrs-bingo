using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("Npcs")]
public class NpcEntity
{
    public int Id { get; set; }
    public int InGameId { get; set; }
    public double? KillsPerHours { get; set; }
    [MaxLength(255)] public string Name { get; set; } = string.Empty;
    [MaxLength(255)] public string Image { get; set; } = string.Empty;
    [ForeignKey("NpcId")] public List<DropInfoEntity> DropInfos { get; set; } = [];
}