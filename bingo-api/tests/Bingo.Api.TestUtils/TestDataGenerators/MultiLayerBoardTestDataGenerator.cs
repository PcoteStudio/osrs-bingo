using Bingo.Api.Core.Features.Boards.MultiLayer.Arguments;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static MultiLayerBoardCreateArguments GenerateMultiLayerBoardCreateArguments()
    {
        return new MultiLayerBoardCreateArguments
        {
            Width = GenerateMultiLayerBoardWidth(),
            Height = GenerateMultiLayerBoardHeight(),
            Depth = GenerateMultiLayerBoardDepth()
        };
    }

    public static int GenerateMultiLayerBoardHeight()
    {
        return Random.Shared.Next(1, 10);
    }

    public static int GenerateMultiLayerBoardWidth()
    {
        return Random.Shared.Next(1, 10);
    }

    public static int GenerateMultiLayerBoardDepth()
    {
        return Random.Shared.Next(2, 6);
    }
}