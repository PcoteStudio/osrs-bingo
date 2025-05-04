using Bingo.Api.Core.Features.Items.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;

namespace Bingo.Api.Core.Features.Items;

public interface IItemFactory
{
    ItemEntity Create(ItemCreateArguments args);
}

public class ItemFactory : IItemFactory
{
    public ItemEntity Create(ItemCreateArguments args)
    {
        var item = new ItemEntity
        {
            Name = args.Name
        };
        if (args.Image is not null && args.Image.Length > 0)
            item.Image = ImageHelper.IFormFileToBase64(args.Image);
        return item;
    }
}