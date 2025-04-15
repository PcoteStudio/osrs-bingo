using Bingo.Api.Core.Features.Items.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Items;

public interface IItemFactory
{
    ItemEntity Create(ItemCreateArguments args);
}

public class ItemFactory : IItemFactory
{
    public ItemEntity Create(ItemCreateArguments args)
    {
        return new ItemEntity
        {
            Name = args.Name,
            Image = args.Image
        };
    }
}