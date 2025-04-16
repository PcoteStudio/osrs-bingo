namespace Bingo.Api.Core.Features.DropInfos.Exceptions;

public class DropInfoAlreadyExistsException(int npcId, int itemId)
    : Exception($"DropInfo between Npc {npcId} and Item {itemId} already exists.")
{
    public int NpcId { get; } = npcId;
    public int ItemId { get; } = itemId;
}