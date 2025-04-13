using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities.Events;

[Serializable]
[Table("Items")]
public class ItemEntity
{
    private NpcEntity? _npc;
    public int Id { get; set; }
    public int InGameId { get; set; }
    [MaxLength(255)] public string Name { get; set; } = string.Empty;
    [MaxLength(255)] public string Image { get; set; } = string.Empty;
    public decimal? DropRate { get; set; }

    public NpcEntity Npc
    {
        get => _npc.ThrowIfNotLoaded();
        set => _npc = value;
    }
}