using Bingo.Api.Core.Features.DropInfos.Arguments;
using Bingo.Api.Core.Features.DropInfos.Exceptions;
using Bingo.Api.Core.Features.Items;
using Bingo.Api.Core.Features.Npcs;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.DropInfos;

public interface IDropInfoService
{
    Task<DropInfoEntity> CreateDropInfoAsync(DropInfoCreateArguments args);
    Task<List<DropInfoEntity>> GetDropInfosAsync();
    Task<DropInfoEntity> RemoveDropInfoAsync(int teamId);
    Task<DropInfoEntity> UpdateDropInfoAsync(int dropInfoId, DropInfoUpdateArguments args);
}

public class DropInfoService(
    IDropInfoFactory dropInfoFactory,
    IDropInfoRepository dropInfoRepository,
    IDropInfoUtil dropInfoUtil,
    IDropInfoServiceHelper dropInfoServiceHelper,
    INpcServiceHelper npcServiceHelper,
    IItemServiceHelper itemServiceHelper,
    ApplicationDbContext dbContext
) : IDropInfoService
{
    public async Task<DropInfoEntity> CreateDropInfoAsync(DropInfoCreateArguments args)
    {
        await dropInfoServiceHelper.EnsureDropInfoDoesNotExistAsync(args.NpcId, args.ItemId);

        var npc = await npcServiceHelper.GetRequiredCompleteByIdAsync(args.NpcId);
        var item = await itemServiceHelper.GetRequiredCompleteByIdAsync(args.ItemId);
        var dropInfo = dropInfoFactory.Create(args);
        dropInfo.Npc = npc;
        dropInfo.Item = item;
        dropInfoUtil.UpdateDropInfoEhc(dropInfo);

        dropInfoRepository.Add(dropInfo);
        await dbContext.SaveChangesAsync();
        return dropInfo;
    }

    public async Task<List<DropInfoEntity>> GetDropInfosAsync()
    {
        return await dropInfoRepository.GetAllCompleteAsync();
    }

    public virtual async Task<DropInfoEntity> RemoveDropInfoAsync(int teamId)
    {
        var dropInfo = await dropInfoRepository.GetCompleteByIdAsync(teamId);
        if (dropInfo is null) throw new DropInfoNotFoundException(teamId);
        dropInfoRepository.Remove(dropInfo);
        return dropInfo;
    }

    public async Task<DropInfoEntity> UpdateDropInfoAsync(int dropInfoId, DropInfoUpdateArguments args)
    {
        var dropInfo = await dropInfoServiceHelper.GetRequiredCompleteByIdAsync(dropInfoId);

        var npc = await npcServiceHelper.GetRequiredCompleteByIdAsync(args.NpcId);
        var item = await itemServiceHelper.GetRequiredCompleteByIdAsync(args.ItemId);
        dropInfoUtil.UpdateDropInfo(dropInfo, args);
        dropInfo.Npc = npc;
        dropInfo.Item = item;
        dropInfoUtil.UpdateDropInfoEhc(dropInfo);

        dbContext.Update(dropInfo);
        await dbContext.SaveChangesAsync();
        return dropInfo;
    }
}