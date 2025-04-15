using Bingo.Api.Core.Features.Npcs.Exceptions;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Npcs;

public interface INpcServiceHelper
{
    Task EnsureNpcDoesNotExistByNameAsync(string name);
    Task<NpcEntity> GetRequiredCompleteNpcAsync(int teamId);
}

public class NpcServiceHelper(
    INpcRepository npcRepository
) : INpcServiceHelper
{
    public async Task EnsureNpcDoesNotExistByNameAsync(string name)
    {
        var player = await npcRepository.GetByNameAsync(name);
        if (player is not null) throw new NpcAlreadyExistsException(player.Name);
    }

    public virtual async Task<NpcEntity> GetRequiredCompleteNpcAsync(int teamId)
    {
        var player = await npcRepository.GetCompleteByIdAsync(teamId);
        if (player is null) throw new NpcNotFoundException(teamId);
        return player;
    }
}