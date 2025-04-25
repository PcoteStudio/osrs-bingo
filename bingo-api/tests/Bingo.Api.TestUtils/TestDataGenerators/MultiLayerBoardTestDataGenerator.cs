using Bingo.Api.Core.Features.Boards.MultiLayer.Arguments;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static MultiLayerBoardCreateArguments GenerateMultiLayerBoardCreateArguments()
    {
        return new MultiLayerBoardCreateArguments
        {
            Width = Random.Shared.Next(1, 10),
            Height = Random.Shared.Next(1, 10),
            Depth = Random.Shared.Next(2, 5)
        };
    }
}