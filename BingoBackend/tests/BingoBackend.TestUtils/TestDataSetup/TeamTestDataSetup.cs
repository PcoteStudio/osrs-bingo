using BingoBackend.Core.Features.Teams;
using BingoBackend.Data.Entities;

namespace BingoBackend.TestUtils.TestDataSetup;

public partial class TestDataSetup
{
    public TestDataSetup AddTeam(Action<TeamEntity>? customizer = null)
    {
        return AddTeam(out _, customizer);
    }

    public TestDataSetup AddTeam(out TeamEntity team, Action<TeamEntity>? customizer = null)
    {
        team = GenerateTeamEntity(customizer);
        dbContext.Teams.Add(team);
        dbContext.SaveChanges();
        return this;
    }

    public TestDataSetup AddTeams(int count, Action<TeamEntity>? customizer = null)
    {
        return AddTeams(count, out _, customizer);
    }

    public TestDataSetup AddTeams(int count, out List<TeamEntity> teams, Action<TeamEntity>? customizer = null)
    {
        teams = Enumerable.Range(0, count).Select(_ =>
        {
            AddTeam(out var team, customizer);
            return team;
        }).ToList();
        return this;
    }

    public static TeamCreateArguments GenerateTeamCreateArguments(Action<TeamCreateArguments>? customizer = null)
    {
        var args = new TeamCreateArguments
        {
            Name = RandomUtil.GetPrefixedRandomHexString("Name_", Random.Shared.Next(5, 25))
        };
        customizer?.Invoke(args);
        return args;
    }

    public static TeamUpdateArguments GenerateTeamUpdateArguments(Action<TeamUpdateArguments>? customizer = null)
    {
        var args = new TeamUpdateArguments
        {
            Name = RandomUtil.GetPrefixedRandomHexString("Name_", Random.Shared.Next(5, 25))
        };
        customizer?.Invoke(args);
        return args;
    }

    private static TeamEntity GenerateTeamEntity(Action<TeamEntity>? customizer = null)
    {
        var teamEntity = new TeamEntity
        {
            Name = RandomUtil.GetPrefixedRandomHexString("Name_", Random.Shared.Next(5, 25))
        };
        customizer?.Invoke(teamEntity);
        return teamEntity;
    }
}