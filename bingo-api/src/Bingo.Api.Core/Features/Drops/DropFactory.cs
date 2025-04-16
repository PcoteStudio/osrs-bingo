using Bingo.Api.Core.Features.Drops.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Drops;

public interface IDropFactory
{
    DropEntity Create(DropCreateArguments args);
}

public class DropFactory : IDropFactory
{
    public DropEntity Create(DropCreateArguments args)
    {
        return new DropEntity
        {
            ItemId = args.ItemId,
            NpcId = args.NpcId,
            DropRate = args.DropRate
        };
    }
}