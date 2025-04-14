using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("Teams")]
public class TeamEntity
{
    private EventEntity? _event;
    private List<PlayerEntity>? _players;

    [Key] public int Id { get; set; }

    public required int EventId { get; set; }

    public EventEntity Event
    {
        get => _event.ThrowIfNotLoaded();
        set => _event = value;
    }

    [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [ForeignKey("TeamId")]
    public List<PlayerEntity> Players
    {
        get => _players.ThrowIfNotLoaded();
        set => _players = value;
    }
}