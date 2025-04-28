using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils.TestDataGenerators;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddNpc(Action<NpcEntity>? customizer = null)
    {
        return AddNpc(out _, customizer);
    }

    public TestDataSetup AddNpc(out NpcEntity npc, Action<NpcEntity>? customizer = null)
    {
        npc = GenerateNpcEntity();
        return SaveEntity(npc, customizer);
    }

    public TestDataSetup AddNpcs(int count, Action<NpcEntity>? customizer = null)
    {
        return AddNpcs(count, out _, customizer);
    }

    public TestDataSetup AddNpcs(int count, out List<NpcEntity> npcs, Action<NpcEntity>? customizer = null)
    {
        npcs = Enumerable.Range(0, count).Select(_ =>
        {
            AddNpc(out var npc, customizer);
            return npc;
        }).ToList();
        return this;
    }

    private NpcEntity GenerateNpcEntity()
    {
        return new NpcEntity
        {
            Name = TestDataGenerator.GenerateNpcName(),
            Image = TestDataGenerator.GenerateNpcImage(),
            KillsPerHour = TestDataGenerator.GenerateNpcKph()
        };
    }
}