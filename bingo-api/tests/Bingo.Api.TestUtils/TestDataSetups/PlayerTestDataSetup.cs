using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddPlayer(Action<PlayerEntity>? customizer = null)
    {
        return AddPlayer(out _, customizer);
    }

    public TestDataSetup AddPlayer(out PlayerEntity player, Action<PlayerEntity>? customizer = null)
    {
        player = GeneratePlayerEntity();
        return SaveEntity(player, customizer);
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

    private PlayerEntity GeneratePlayerEntity()
    {
        var player = new PlayerEntity
        {
            Name = RandomUtil.GetPrefixedRandomHexString("PName_", Random.Shared.Next(5, 25))
        };
        var lastTeamAdded = GetLast<TeamEntity>();
        if (lastTeamAdded is not null) player.Teams.Add(lastTeamAdded);
        return player;
    }
}