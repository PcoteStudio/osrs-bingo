using Bingo.Api.Core.Features.Npcs.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;
using Bingo.Api.Web.Npcs;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static string GenerateNpcName()
    {
        return RandomUtil.GetPrefixedRandomHexString("NName_", 5, 25);
    }

    public static string GenerateNpcImage()
    {
        return RandomUtil.GetPrefixedRandomHexString("Image_", 5, 50);
    }

    public static double GenerateNpcKph()
    {
        return Random.Shared.NextDouble() * 200;
    }

    public static NpcEntity GenerateNpcEntity()
    {
        return new NpcEntity
        {
            Id = Random.Shared.Next(),
            Name = GenerateNpcName(),
            Image = GenerateNpcImage(),
            Drops = []
        };
    }

    public static List<NpcEntity> GenerateNpcEntities(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateNpcEntity()).ToList();
    }

    public static NpcResponse GenerateNpcResponse()
    {
        return new NpcResponse
        {
            Id = Random.Shared.Next(),
            Name = GenerateNpcName(),
            Image = GenerateNpcImage(),
            KillsPerHour = GenerateNpcKph(),
            Drops = []
        };
    }

    public static List<NpcResponse> GenerateNpcResponses(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateNpcResponse()).ToList();
    }

    public static NpcCreateArguments GenerateNpcCreateArguments()
    {
        return new NpcCreateArguments
        {
            Name = GenerateNpcName(),
            Image = GenerateNpcImage(),
            KillsPerHour = GenerateNpcKph()
        };
    }

    public static NpcUpdateArguments GenerateNpcUpdateArguments()
    {
        return new NpcUpdateArguments
        {
            Name = GenerateNpcName(),
            Image = GenerateNpcImage(),
            KillsPerHour = GenerateNpcKph()
        };
    }
}