using Bingo.Api.Data.Entities;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddBoardLayer(Action<BoardLayerEntity>? customizer = null)
    {
        return AddBoardLayer(out _, customizer);
    }

    public TestDataSetup AddBoardLayer(out BoardLayerEntity layer,
        Action<BoardLayerEntity>? customizer = null)
    {
        layer = GenerateBoardLayerEntity();
        return SaveEntity(layer, customizer);
    }

    public TestDataSetup AddExpectedBoardLayers(Action<BoardLayerEntity>? customizer = null)
    {
        AddExpectedBoardLayers(out _, customizer);
        return this;
    }

    public TestDataSetup AddExpectedBoardLayers(out List<BoardLayerEntity> layers,
        Action<BoardLayerEntity>? customizer = null)
    {
        var board = GetRequiredLast<MultiLayerBoardEntity>();
        layers = new List<BoardLayerEntity>();
        for (var i = board.Layers.Count; i < board.Depth; i++)
        {
            AddBoardLayer(out var layer, customizer);
            layers.Add(layer);
        }

        return this;
    }

    private BoardLayerEntity GenerateBoardLayerEntity()
    {
        var board = GetRequiredLast<MultiLayerBoardEntity>();
        var layer = new BoardLayerEntity
        {
            BoardId = board.Id,
            Board = board,
            Height = board.Height,
            Width = board.Width,
            Tiles = []
        };
        return layer;
    }
}