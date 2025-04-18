using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static string GenerateEventName()
    {
        return RandomUtil.GetPrefixedRandomHexString("EName_", 5, 25);
    }

    public static EventEntity GenerateEventEntity()
    {
        return new EventEntity
        {
            Id = Random.Shared.Next(),
            Name = GenerateEventName(),
            Administrators = [],
            Teams = []
        };
    }
}