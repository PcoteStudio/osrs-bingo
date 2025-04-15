using Bingo.Api.Core.Features.Npcs.Exceptions;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Npcs;

public interface INpcServiceHelper
{
    Task EnsureNpcDoesNotExistByNameAsync(string name);
    Task<NpcEntity> GetRequiredCompleteByIdAsync(int npcId);
}

public class NpcServiceHelper(
    INpcRepository npcRepository
) : INpcServiceHelper
{
    public async Task EnsureNpcDoesNotExistByNameAsync(string name)
    {
        var npc = await npcRepository.GetByNameAsync(name);
        if (npc is not null) throw new NpcAlreadyExistsException(npc.Name);
    }

    public virtual async Task<NpcEntity> GetRequiredCompleteByIdAsync(int npcId)
    {
        var player = await npcRepository.GetCompleteByIdAsync(npcId);
        if (player is null) throw new NpcNotFoundException(npcId);
        return player;
    }
}