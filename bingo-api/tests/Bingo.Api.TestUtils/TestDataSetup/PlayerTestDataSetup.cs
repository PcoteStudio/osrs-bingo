using Bingo.Api.Data.Entities;

namespace Bingo.Api.TestUtils.TestDataSetup;

public partial class TestDataSetup
{
    public TestDataSetup AddPlayer(Action<PlayerEntity>? customizer = null)
    {
        return AddPlayer(out _, customizer);
    }

    public TestDataSetup AddPlayer(out PlayerEntity player, Action<PlayerEntity>? customizer = null)
    {
        player = GeneratePlayerEntity(customizer);
        dbContext.Players.Add(player);
        dbContext.SaveChanges();
        return this;
    }

    public TestDataSetup AddPlayers(int count, Action<PlayerEntity>? customizer = null)
    {
        return AddPlayers(count, out _, customizer);
    }

    public TestDataSetup AddPlayers(int count, out List<PlayerEntity> players, Action<PlayerEntity>? customizer = null)
    {
        players = Enumerable.Range(0, count).Select(_ =>
        {
            AddPlayer(out var player, customizer);
            return player;
        }).ToList();
        return this;
    }

    private static PlayerEntity GeneratePlayerEntity(Action<PlayerEntity>? customizer)
    {
        var player = new PlayerEntity
        {
            Name = GeneratePlayerName()
        };
        customizer?.Invoke(player);
        return player;
    }

    private static string GeneratePlayerName()
    {
        return RandomUtil.GetPrefixedRandomHexString("PName_", Random.Shared.Next(5, 25));
    }
}