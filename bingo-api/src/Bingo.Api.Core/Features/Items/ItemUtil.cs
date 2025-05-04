using Bingo.Api.Core.Features.Items.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;

namespace Bingo.Api.Core.Features.Items;

public interface IItemUtil
{
    void UpdateItem(ItemEntity item, ItemUpdateArguments args);
}

public class ItemUtil : IItemUtil
{
    public void UpdateItem(ItemEntity item, ItemUpdateArguments args)
    {
        item.Name = args.Name;
        if (args.Image is not null && args.Image.Length > 0)
            item.Image = ImageHelper.IFormFileToBase64(args.Image);
        else
            item.Image = null;
    }
}