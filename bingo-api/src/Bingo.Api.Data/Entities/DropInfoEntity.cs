using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("DropInfos")]
public class DropInfoEntity
{
    private ItemEntity? _item;
    private NpcEntity? _npc;

    public int Id { get; set; }
    public int NpcId { get; set; }
    public int ItemId { get; set; }
    public double? DropRate { get; set; }
    public double? Ehc { get; set; }

    public NpcEntity Npc
    {
        get => _npc.ThrowIfNotLoaded();
        set => _npc = value;
    }

    public ItemEntity Item
    {
        get => _item.ThrowIfNotLoaded();
        set => _item = value;
    }
}