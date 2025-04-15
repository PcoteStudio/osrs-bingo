using Bingo.Api.Core.Features.Items.Arguments;
using Bingo.Api.Core.Features.Items.Exceptions;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Items;

public interface IItemService
{
    Task<ItemEntity> CreateItemAsync(ItemCreateArguments args);
    Task<List<ItemEntity>> GetItemsAsync();
    Task<ItemEntity> RemoveItemAsync(int teamId);
    Task<ItemEntity> UpdateItemAsync(int itemId, ItemUpdateArguments args);
}

public class ItemService(
    IItemFactory itemFactory,
    IItemRepository itemRepository,
    IItemUtil itemUtil,
    IItemServiceHelper itemServiceHelper,
    ApplicationDbContext dbContext
) : IItemService
{
    public async Task<ItemEntity> CreateItemAsync(ItemCreateArguments args)
    {
        await itemServiceHelper.EnsureItemDoesNotExistByNameAsync(args.Name);
        var item = itemFactory.Create(args);
        itemRepository.Add(item);
        await dbContext.SaveChangesAsync();
        return item;
    }

    public async Task<List<ItemEntity>> GetItemsAsync()
    {
        return await itemRepository.GetAllCompleteAsync();
    }

    public virtual async Task<ItemEntity> RemoveItemAsync(int teamId)
    {
        var item = await itemRepository.GetCompleteByIdAsync(teamId);
        if (item is null) throw new ItemNotFoundException(teamId);
        itemRepository.Remove(item);
        return item;
    }

    public async Task<ItemEntity> UpdateItemAsync(int itemId, ItemUpdateArguments args)
    {
        var itemEntity = await itemServiceHelper.GetRequiredCompleteByIdAsync(itemId);
        itemUtil.UpdateItem(itemEntity, args);
        dbContext.Update(itemEntity);
        await dbContext.SaveChangesAsync();
        return itemEntity;
    }
}