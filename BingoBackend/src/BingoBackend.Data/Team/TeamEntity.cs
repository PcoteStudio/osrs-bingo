using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Data.Team;

public class TeamEntity
{
    [Key] public int Id { get; set; }

    [MaxLength(255)] public string Name { get; set; } = string.Empty;
}