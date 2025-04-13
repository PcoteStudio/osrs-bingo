using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Data.Entities.Events;

[Serializable]
[Table("Players")]
[Index(nameof(Name), IsUnique = true)]
public class PlayerEntity
{
    private List<TeamEntity>? _teams;
    public int Id { get; set; }

    [MaxLength(255)] public required string Name { get; set; } = string.Empty;

    [ForeignKey("PlayerId")]
    public List<TeamEntity>? Teams
    {
        get => _teams.ThrowIfNotLoaded();
        set => _teams = value;
    }
}