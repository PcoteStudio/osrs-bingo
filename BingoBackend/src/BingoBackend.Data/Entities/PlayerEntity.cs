using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BingoBackend.Data.Entities;

[Table("Players")]
[Index(nameof(Name), IsUnique = true)]
public class PlayerEntity
{
    [Key] public int Id { get; set; }

    [MaxLength(255)] [Required] public string Name { get; set; } = string.Empty;

    [ForeignKey("PlayerId")] public List<TeamEntity> Teams { get; set; } = [];
}