using Bingo.Api.Core.Features.Players.Arguments;
using Bingo.Api.Shared;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static string GeneratePlayerName()
    {
        return RandomUtil.GetPrefixedRandomHexString("PName_", 5, 20);
    }

    public static PlayerUpdateArguments GeneratePlayerUpdateArguments()
    {
        return new PlayerUpdateArguments
        {
            Name = GeneratePlayerName()
        };
    }
}