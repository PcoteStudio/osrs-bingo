using Bingo.Api.Shared;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static string GeneratePlayerName()
    {
        return RandomUtil.GetPrefixedRandomHexString("PName_", 5, 20);
    }
}