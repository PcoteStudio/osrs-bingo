using Bingo.Api.Core.Features.Drops.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Drops;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static double GenerateDropRate()
    {
        return Random.Shared.NextDouble() * 10_000;
    }

    public static double GenerateDropEhc()
    {
        return Random.Shared.NextDouble() * 100;
    }

    public static DropEntity GenerateDropEntity()
    {
        return new DropEntity
        {
            Id = Random.Shared.Next(),
            NpcId = Random.Shared.Next(),
            ItemId = Random.Shared.Next(),
            DropRate = GenerateDropRate(),
            Ehc = GenerateDropEhc()
        };
    }

    public static List<DropEntity> GenerateDropEntities(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateDropEntity()).ToList();
    }

    public static DropResponse GenerateDropResponse()
    {
        return new DropResponse
        {
            Id = Random.Shared.Next(),
            NpcId = Random.Shared.Next(),
            ItemId = Random.Shared.Next(),
            DropRate = GenerateDropRate(),
            Ehc = GenerateDropEhc(),
            Item = GenerateItemResponse(),
            Npc = GenerateNpcResponse()
        };
    }

    public static List<DropResponse> GenerateDropResponses(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateDropResponse()).ToList();
    }

    public static DropCreateArguments GenerateDropCreateArguments(int? npcId = null, int? itemId = null)
    {
        return new DropCreateArguments
        {
            NpcId = npcId ?? Random.Shared.Next(),
            ItemId = itemId ?? Random.Shared.Next(),
            DropRate = GenerateDropRate()
        };
    }

    public static DropUpdateArguments GenerateDropUpdateArguments(int? npcId = null, int? itemId = null)
    {
        return new DropUpdateArguments
        {
            NpcId = npcId ?? Random.Shared.Next(),
            ItemId = itemId ?? Random.Shared.Next(),
            DropRate = GenerateDropRate()
        };
    }
}