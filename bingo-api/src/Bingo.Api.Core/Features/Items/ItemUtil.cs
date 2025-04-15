using Bingo.Api.Core.Features.Items.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Items;

public interface IItemUtil
{
    void UpdateItem(ItemEntity npc, ItemUpdateArguments args);
}

public class ItemUtil : IItemUtil
{
    public void UpdateItem(ItemEntity npc, ItemUpdateArguments args)
    {
        npc.Name = args.Name;
        npc.Image = args.Image;
    }
}