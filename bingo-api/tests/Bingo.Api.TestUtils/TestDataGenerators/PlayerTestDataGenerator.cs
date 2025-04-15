using Bingo.Api.Shared;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static string GeneratePlayerName()
    {
        return RandomUtil.GetPrefixedRandomHexString("PName_", Random.Shared.Next(5, 25));
    }
}