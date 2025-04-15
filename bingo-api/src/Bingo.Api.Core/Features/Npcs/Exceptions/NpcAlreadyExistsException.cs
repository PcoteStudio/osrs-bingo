namespace Bingo.Api.Core.Features.Npcs.Exceptions;

public class NpcAlreadyExistsException(string npcName) : Exception($"Npc {npcName} already exists.")
{
    public string NpcName { get; } = npcName;
}