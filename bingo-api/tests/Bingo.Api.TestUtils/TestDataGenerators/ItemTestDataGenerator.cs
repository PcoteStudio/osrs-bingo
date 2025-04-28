using Bingo.Api.Core.Features.Items.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;
using Bingo.Api.Web.Items;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static string GenerateItemName()
    {
        return RandomUtil.GetPrefixedRandomHexString("IName_", 5, 25);
    }

    public static string GenerateItemImage()
    {
        return RandomUtil.GetPrefixedRandomHexString("Image_", 5, 50);
    }

    public static ItemEntity GenerateItemEntity()
    {
        return new ItemEntity
        {
            Id = Random.Shared.Next(),
            Name = GenerateItemName(),
            Image = GenerateItemImage(),
            Drops = []
        };
    }

    public static List<ItemEntity> GenerateItemEntities(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateItemEntity()).ToList();
    }

    public static ItemResponse GenerateItemResponse()
    {
        return new ItemResponse
        {
            Id = Random.Shared.Next(),
            Name = GenerateItemName(),
            Image = GenerateItemImage()
        };
    }

    public static List<ItemResponse> GenerateItemResponses(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateItemResponse()).ToList();
    }

    public static ItemCreateArguments GenerateItemCreateArguments()
    {
        return new ItemCreateArguments
        {
            Name = GenerateItemName(),
            Image = GenerateItemImage()
        };
    }

    public static ItemUpdateArguments GenerateItemUpdateArguments()
    {
        return new ItemUpdateArguments
        {
            Name = GenerateItemName(),
            Image = GenerateItemImage()
        };
    }
}