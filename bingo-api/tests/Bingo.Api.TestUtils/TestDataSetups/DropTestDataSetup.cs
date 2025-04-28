using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils.TestDataGenerators;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddDrop(Action<DropEntity>? customizer = null)
    {
        return AddDrop(out _, customizer);
    }

    public TestDataSetup AddDrop(out DropEntity drop, Action<DropEntity>? customizer = null)
    {
        drop = GenerateDropEntity();
        return SaveEntity(drop, customizer);
    }

    public TestDataSetup AddDrops(int count, Action<DropEntity>? customizer = null)
    {
        return AddDrops(count, out _, customizer);
    }

    public TestDataSetup AddDrops(int count, out List<DropEntity> drops, Action<DropEntity>? customizer = null)
    {
        drops = Enumerable.Range(0, count).Select(_ =>
        {
            AddDrop(out var drop, customizer);
            return drop;
        }).ToList();
        return this;
    }

    private DropEntity GenerateDropEntity()
    {
        var npc = GetRequiredLast<NpcEntity>();
        var item = GetRequiredLast<ItemEntity>();
        return new DropEntity
        {
            NpcId = npc.Id,
            Npc = npc,
            ItemId = item.Id,
            Item = item,
            DropRate = TestDataGenerator.GenerateDropRate(),
            Ehc = TestDataGenerator.GenerateDropEhc()
        };
    }
}