using Bingo.Api.Core.Features.DropInfos.Exceptions;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.DropInfos;

public interface IDropInfoServiceHelper
{
    Task<DropInfoEntity> GetRequiredCompleteByIdAsync(int dropInfoId);
    Task EnsureDropInfoDoesNotExistAsync(int npcId, int itemId);
}

public class DropInfoServiceHelper(
    IDropInfoRepository npcRepository
) : IDropInfoServiceHelper
{
    public virtual async Task<DropInfoEntity> GetRequiredCompleteByIdAsync(int dropInfoId)
    {
        var dropInfo = await npcRepository.GetCompleteByIdAsync(dropInfoId);
        if (dropInfo is null) throw new DropInfoNotFoundException(dropInfoId);
        return dropInfo;
    }

    public async Task EnsureDropInfoDoesNotExistAsync(int npcId, int itemId)
    {
        var dropInfo = await npcRepository.GetByNpcIdAndItemIdAsync(npcId, itemId);
        if (dropInfo is not null) throw new DropInfoAlreadyExistsException(npcId, itemId);
    }
}