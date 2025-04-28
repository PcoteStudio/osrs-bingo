using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils.TestDataGenerators;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddItem(Action<ItemEntity>? customizer = null)
    {
        return AddItem(out _, customizer);
    }

    public TestDataSetup AddItem(out ItemEntity item, Action<ItemEntity>? customizer = null)
    {
        item = GenerateItemEntity();
        return SaveEntity(item, customizer);
    }

    public TestDataSetup AddItems(int count, Action<ItemEntity>? customizer = null)
    {
        return AddItems(count, out _, customizer);
    }

    public TestDataSetup AddItems(int count, out List<ItemEntity> items, Action<ItemEntity>? customizer = null)
    {
        items = Enumerable.Range(0, count).Select(_ =>
        {
            AddItem(out var item, customizer);
            return item;
        }).ToList();
        return this;
    }

    private ItemEntity GenerateItemEntity()
    {
        return new ItemEntity
        {
            Name = TestDataGenerator.GenerateItemName(),
            Image = TestDataGenerator.GenerateItemImage()
        };
    }
}