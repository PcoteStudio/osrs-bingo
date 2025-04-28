using Bingo.Api.Core.Features.Drops.Arguments;
using Bingo.Api.Core.Features.Drops.Exceptions;
using Bingo.Api.Core.Features.Items;
using Bingo.Api.Core.Features.Npcs;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Drops;

public interface IDropService
{
    Task<DropEntity> CreateDropAsync(DropCreateArguments args);
    Task<List<DropEntity>> GetDropsAsync();
    Task<DropEntity> RemoveDropAsync(int teamId);
    Task<DropEntity> UpdateDropAsync(int dropId, DropUpdateArguments args);
}

public class DropService(
    IDropFactory dropFactory,
    IDropRepository dropRepository,
    IDropUtil dropUtil,
    IDropServiceHelper dropServiceHelper,
    INpcServiceHelper npcServiceHelper,
    IItemServiceHelper itemServiceHelper,
    ApplicationDbContext dbContext
) : IDropService
{
    public async Task<DropEntity> CreateDropAsync(DropCreateArguments args)
    {
        await dropServiceHelper.EnsureDropDoesNotExistAsync(args.NpcId, args.ItemId);

        var npc = await npcServiceHelper.GetRequiredCompleteByIdAsync(args.NpcId);
        var item = await itemServiceHelper.GetRequiredCompleteByIdAsync(args.ItemId);
        var drop = dropFactory.Create(args);
        drop.Npc = npc;
        drop.Item = item;
        dropUtil.UpdateDropEhc(drop);

        dropRepository.Add(drop);
        await dbContext.SaveChangesAsync();
        return drop;
    }

    public async Task<List<DropEntity>> GetDropsAsync()
    {
        return await dropRepository.GetAllCompleteAsync();
    }

    public virtual async Task<DropEntity> RemoveDropAsync(int teamId)
    {
        var drop = await dropRepository.GetCompleteByIdAsync(teamId);
        if (drop is null) throw new DropNotFoundException(teamId);
        dropRepository.Remove(drop);
        return drop;
    }

    public async Task<DropEntity> UpdateDropAsync(int dropId, DropUpdateArguments args)
    {
        var drop = await dropServiceHelper.GetRequiredCompleteByIdAsync(dropId);
        await dropServiceHelper.EnsureDropDoesNotExistAsync(args.NpcId, args.ItemId);

        var npc = await npcServiceHelper.GetRequiredCompleteByIdAsync(args.NpcId);
        var item = await itemServiceHelper.GetRequiredCompleteByIdAsync(args.ItemId);
        dropUtil.UpdateDrop(drop, args);
        drop.Npc = npc;
        drop.Item = item;
        dropUtil.UpdateDropEhc(drop);

        dbContext.Update(drop);
        await dbContext.SaveChangesAsync();
        return drop;
    }
}