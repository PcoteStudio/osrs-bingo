using Bingo.Api.Core.Features.Items.Arguments;
using Bingo.Api.Data.Entities;

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
        item.Image = args.Image;
    }
}