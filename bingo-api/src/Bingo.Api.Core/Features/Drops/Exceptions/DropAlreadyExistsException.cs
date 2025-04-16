namespace Bingo.Api.Core.Features.Drops.Exceptions;

public class DropAlreadyExistsException(int npcId, int itemId)
    : Exception($"Drop between Npc {npcId} and Item {itemId} already exists.")
{
    public int NpcId { get; } = npcId;
    public int ItemId { get; } = itemId;
}