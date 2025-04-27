using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils.TestDataGenerators;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddMultiLayerBoard(Action<MultiLayerBoardEntity>? customizer = null)
    {
        return AddMultiLayerBoard(out _, customizer);
    }

    public TestDataSetup AddMultiLayerBoard(out MultiLayerBoardEntity board,
        Action<MultiLayerBoardEntity>? customizer = null)
    {
        board = GenerateMultiLayerBoardEntity();
        SaveEntity(board, customizer);
        return AddExpectedBoardLayers();
    }

    private MultiLayerBoardEntity GenerateMultiLayerBoardEntity()
    {
        var eventEntity = GetRequiredLast<EventEntity>();
        var board = new MultiLayerBoardEntity
        {
            EventId = eventEntity.Id,
            Event = eventEntity,
            Height = TestDataGenerator.GenerateMultiLayerBoardHeight(),
            Width = TestDataGenerator.GenerateMultiLayerBoardWidth(),
            Depth = TestDataGenerator.GenerateMultiLayerBoardDepth()
        };
        return board;
    }
}