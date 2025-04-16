using Bingo.Api.Core.Features.DropInfos.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.DropInfos;

public interface IDropInfoFactory
{
    DropInfoEntity Create(DropInfoCreateArguments args);
}

public class DropInfoFactory : IDropInfoFactory
{
    public DropInfoEntity Create(DropInfoCreateArguments args)
    {
        return new DropInfoEntity
        {
            ItemId = args.ItemId,
            NpcId = args.NpcId,
            DropRate = args.DropRate
        };
    }
}