using Bingo.Api.Core.Features.Items.Exceptions;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Items;

public interface IItemServiceHelper
{
    Task EnsureItemDoesNotExistByNameAsync(string name);
    Task<ItemEntity> GetRequiredCompleteByIdAsync(int itemId);
}

public class ItemServiceHelper(
    IItemRepository npcRepository
) : IItemServiceHelper
{
    public async Task EnsureItemDoesNotExistByNameAsync(string name)
    {
        var item = await npcRepository.GetByNameAsync(name);
        if (item is not null) throw new ItemAlreadyExistsException(item.Name);
    }

    public virtual async Task<ItemEntity> GetRequiredCompleteByIdAsync(int itemId)
    {
        var player = await npcRepository.GetCompleteByIdAsync(itemId);
        if (player is null) throw new ItemNotFoundException(itemId);
        return player;
    }
}