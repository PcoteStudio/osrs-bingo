using Bingo.Api.Data.Entities.Events;
using Bingo.Api.TestUtils.TestDataGenerators;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddTeam(Action<TeamEntity>? customizer = null)
    {
        return AddTeam(out _, customizer);
    }

    public TestDataSetup AddTeam(out TeamEntity team, Action<TeamEntity>? customizer = null)
    {
        team = GenerateTeamEntity();
        return SaveEntity(team, customizer);
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

    private TeamEntity GenerateTeamEntity()
    {
        var eventEntity = GetRequiredLast<EventEntity>();
        var team = new TeamEntity
        {
            EventId = eventEntity.Id,
            Event = eventEntity,
            Name = TestDataGenerator.GenerateTeamName(),
            Players = []
        };
        return team;
    }
}