using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("Npcs")]
[Index(nameof(Name), IsUnique = true)]
public class NpcEntity
{
    public int Id { get; set; }
    public double? KillsPerHour { get; set; }
    [MaxLength(255)] public string Name { get; set; } = string.Empty;
    [MaxLength(255)] public string Image { get; set; } = string.Empty;
    [ForeignKey("NpcId")] public List<DropEntity> Drops { get; set; } = [];
}