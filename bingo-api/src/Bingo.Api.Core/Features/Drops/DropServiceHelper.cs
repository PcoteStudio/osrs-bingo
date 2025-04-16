using Bingo.Api.Core.Features.Drops.Exceptions;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Drops;

public interface IDropServiceHelper
{
    Task<DropEntity> GetRequiredCompleteByIdAsync(int dropId);
    Task EnsureDropDoesNotExistAsync(int npcId, int itemId);
}

public class DropServiceHelper(
    IDropRepository npcRepository
) : IDropServiceHelper
{
    public virtual async Task<DropEntity> GetRequiredCompleteByIdAsync(int dropId)
    {
        var drop = await npcRepository.GetCompleteByIdAsync(dropId);
        if (drop is null) throw new DropNotFoundException(dropId);
        return drop;
    }

    public async Task EnsureDropDoesNotExistAsync(int npcId, int itemId)
    {
        var drop = await npcRepository.GetByNpcIdAndItemIdAsync(npcId, itemId);
        if (drop is not null) throw new DropAlreadyExistsException(npcId, itemId);
    }
}