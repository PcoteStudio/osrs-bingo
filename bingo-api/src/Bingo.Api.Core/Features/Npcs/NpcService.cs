using Bingo.Api.Core.Features.Npcs.Arguments;
using Bingo.Api.Core.Features.Npcs.Exceptions;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Npcs;

public interface INpcService
{
    Task<NpcEntity> CreateNpcAsync(NpcCreateArguments args);
    Task<List<NpcEntity>> GetNpcsAsync();
    Task<NpcEntity> RemoveNpcAsync(int teamId);
    Task<NpcEntity> UpdateNpcAsync(int npcId, NpcUpdateArguments args);
}

public class NpcService(
    INpcFactory npcFactory,
    INpcRepository npcRepository,
    INpcUtil npcUtil,
    INpcServiceHelper npcServiceHelper,
    ApplicationDbContext dbContext
) : INpcService
{
    public async Task<NpcEntity> CreateNpcAsync(NpcCreateArguments args)
    {
        await npcServiceHelper.EnsureNpcDoesNotExistByNameAsync(args.Name);
        var npc = npcFactory.Create(args);
        npcRepository.Add(npc);
        await dbContext.SaveChangesAsync();
        return npc;
    }

    public async Task<List<NpcEntity>> GetNpcsAsync()
    {
        return await npcRepository.GetAllCompleteAsync();
    }

    public virtual async Task<NpcEntity> RemoveNpcAsync(int teamId)
    {
        var npc = await npcRepository.GetCompleteByIdAsync(teamId);
        if (npc is null) throw new NpcNotFoundException(teamId);
        npcRepository.Remove(npc);
        return npc;
    }

    public async Task<NpcEntity> UpdateNpcAsync(int npcId, NpcUpdateArguments args)
    {
        var npcEntity = await npcServiceHelper.GetRequiredCompleteNpcAsync(npcId);
        npcUtil.UpdateNpc(npcEntity, args);
        dbContext.Update(npcEntity);
        await dbContext.SaveChangesAsync();
        return npcEntity;
    }
}