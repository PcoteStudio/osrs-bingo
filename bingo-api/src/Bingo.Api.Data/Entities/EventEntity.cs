using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("Events")]
public class EventEntity
{
    private List<UserEntity>? _administrators;
    private List<TeamEntity>? _teams;

    public int Id { get; set; }

    public int? BoardId { get; set; }

    [ForeignKey("EventId")] public BoardEntity? Board { get; set; }

    [MaxLength(255)] public string Name { get; set; } = string.Empty;

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    [ForeignKey("EventId")]
    public List<TeamEntity> Teams
    {
        get => _teams.ThrowIfNotLoaded();
        set => _teams = value;
    }

    [ForeignKey("EventId")]
    public List<UserEntity> Administrators
    {
        get => _administrators.ThrowIfNotLoaded();
        set => _administrators = value;
    }
}