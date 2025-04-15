namespace Bingo.Api.Core.Features.Npcs.Exceptions;

public class NpcNotFoundException(int npcId) : Exception($"Npc {npcId} not found.")
{
    public int NpcId { get; } = npcId;
}